using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaWithAspId3AndIdSvr4.Configuration
{
    public class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "spa",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RedirectUris = new List<string>
                    {
                        "http://localhost:1861/spa/callback.html"
                    },
                    AllowedScopes = new List<string>
                    {
                        "openid", "profile",
                        "api.todo"
                    }
                }
            };
        }
    }
}
