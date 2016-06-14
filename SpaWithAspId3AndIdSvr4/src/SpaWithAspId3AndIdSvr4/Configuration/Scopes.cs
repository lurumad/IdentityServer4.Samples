using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaWithAspId3AndIdSvr4.Configuration
{
    public class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            return new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                new Scope
                {
                    Name = "api.todo",
                    DisplayName = "TODO API",
                    Description = "TODO features and data",
                    Type = ScopeType.Resource,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role")
                    }
                },
            };
        }
    }
}
