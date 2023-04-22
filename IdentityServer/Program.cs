using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using IdentityServer;
using IdentityServerHost.Quickstart.UI;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

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

// Add Azure AD authentication
builder.Services.AddAuthentication()
    .AddOpenIdConnect("AzureAD", "Office 365", options =>
    {
        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

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