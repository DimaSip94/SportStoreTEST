using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportsStore.Infrastructure
{
    public class LocatinClaimsProvider : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if(principal!=null && !principal.HasClaim(x => x.Type == ClaimTypes.PostalCode))
            {
                ClaimsIdentity claimsIdentity = principal.Identity as ClaimsIdentity;
                if(claimsIdentity!=null && claimsIdentity.IsAuthenticated && claimsIdentity.Name != null)
                {
                    if (claimsIdentity.Name.ToLower() == "admin")
                    {
                        claimsIdentity.AddClaims(new Claim[] {
                            CreateClaim(ClaimTypes.PostalCode, "NY 10036"),
                            CreateClaim(ClaimTypes.StateOrProvince, "NY")
                        });
                    }
                }
            }
            return Task.FromResult(principal);
        }

        private static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String, "RemoteClaims");
        }
    }
}
