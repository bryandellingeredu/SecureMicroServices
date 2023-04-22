using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                    new Client
                    {
                        ClientId = "movieClient",
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        },
                        AllowedScopes = {"movieAPI"}
                    },
                   new Client
{
    ClientId = "edf44846-5e26-409e-b4de-59676a88e0c3", // Use a custom ClientId instead of the one from Azure AD App
    ClientName =  "Movies MVC Web App",
    AllowedGrantTypes = GrantTypes.Implicit,
        AllowAccessTokensViaBrowser = true,
    RequirePkce = false,
    AllowRememberConsent = false,
    RedirectUris = new List<string>()
    {
        "https://localhost:5002/signin-oidc"
    },
    PostLogoutRedirectUris = new List<string>()
    {
        "https://localhost:5002/signout-callback-oidc"
    },
    ClientSecrets =
    {
        new Secret("FJd8Q~-wGZfozfIfPdb9z5W_i1fGy4xZLBNMXdjW".Sha256()) // Keep the same ClientSecret from Azure AD App
    },
    AllowedScopes = new List<string>
    {
        IdentityServerConstants.StandardScopes.OpenId,
        IdentityServerConstants.StandardScopes.Profile,
        IdentityServerConstants.StandardScopes.Email,
        "movieAPI",
    },
    Enabled = true // Make sure the client is enabled
}
            };
        public static IEnumerable<ApiScope> ApiScopes =>
           new ApiScope[]
           {
               new ApiScope("movieAPI", "Movie API")
           };
        public static IEnumerable<ApiResource> ApiResources =>
         new ApiResource[]
        {

        };
        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
       {
           new IdentityResources.OpenId(),
           new IdentityResources.Profile(),
           new IdentityResources.Email(),
       };

    }
}