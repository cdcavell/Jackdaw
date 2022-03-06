using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace Jackdaw.IdentityServer
{
    /// <summary>
    /// Migrations Assembly Class
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.2 | 03/06/2022 | Duende IdentityServer Integration |~ 
    /// </revision>
    public static class Config
    {
        /// <value>IEnumerable&lt;IdentityResource&gt;</value>
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        /// <value>IEnumerable&lt;ApiScope&gt;</value>
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api1", "MyAPI")
            };

        /// <value>IEnumerable&lt;ApiResource&gt;</value>
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
            };

        /// <value>IEnumerable&lt;Client&gt;</value>
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                // machine-to-machine client (from quickstart 1)
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    // scopes that client has access to
                    AllowedScopes = { "api1" }
                },
                // interactive ASP.NET Core Web App
                new Client
                {
                    ClientId = "web",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    // where to redirect after login
                    RedirectUris = { "https://localhost:5002/signin-oidc" },

                    // where to redirect after logout
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                    AllowOfflineAccess = true,

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                }
            };
    }
}
