using IdentityServer;
using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
    logging.SetMinimumLevel(LogLevel.Trace);
});

builder.Services.AddControllersWithViews();

// Configure IdentityServer services
builder.Services.AddIdentityServer()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddDeveloperSigningCredential();

builder.Services.AddTransient<IProfileService, CustomProfileService>();
builder.Services.AddDistributedMemoryCache();

// Add Azure AD authentication
builder.Services.AddAuthentication()
    .AddOpenIdConnect("AzureAD", "Office 365", options =>
    {
        options.ClaimActions.MapAllExcept("iss", "nbf", "exp", "aud", "nonce", "iat", "c_hash");
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
        options.ClaimActions.MapUniqueJsonKey("preferred_username", "preferred_username");
        options.ClaimActions.MapUniqueJsonKey("email", "email");
        options.ClientId = "edf44846-5e26-409e-b4de-59676a88e0c3";
        options.ClientSecret = "FJd8Q~-wGZfozfIfPdb9z5W_i1fGy4xZLBNMXdjW"; // Add your client secret here
        options.Authority = "https://login.microsoftonline.com/44f5f615-327a-4d5a-86d5-c9251297d7e4/v2.0";
        options.ResponseType = "code";
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");

        options.CallbackPath = "/signin-oidc";

        options.SaveTokens = true;
        options.TokenValidationParameters.NameClaimType = "name";
        options.Events.OnUserInformationReceived = async ctx =>
        {
         
            var email = ctx.User.RootElement.GetString("email");
            var name = ctx.User.RootElement.GetString("name");
            if (!string.IsNullOrEmpty(email))
            {
                if (!string.IsNullOrEmpty(email))
                {
                    EmailClaimStorage.EmailClaims.TryAdd(name, email);
                }
            }
        };

    });

var app = builder.Build();

// Add IdentityServer middleware
app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthentication(); // Add this line
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});



app.Run();