using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using IdentityServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Configure IdentityServer services
builder.Services.AddIdentityServer()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes)
     .AddInMemoryIdentityResources(Config.IdentityResources)
     .AddTestUsers((List<TestUser>)Config.TestUsers)
    .AddDeveloperSigningCredential();

var app = builder.Build();

// Add IdentityServer middleware
app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();