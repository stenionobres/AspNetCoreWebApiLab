using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using AspNetCoreWebApiLab.Api.Models.V1;

namespace AspNetCoreWebApiLab.Api.Services
{
    public class RoleClaimService
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public RoleClaimService(RoleManager<IdentityRole<int>> roleManager)
        {
            _roleManager = roleManager;
        }

        public void Associate(int roleId, ClaimModel claim)
        {
            var identityRole = _roleManager.Roles.FirstOrDefault(r => r.Id.Equals(roleId));
            var identityClaim = new Claim(claim.Type, claim.Value);
            var identityResult = _roleManager.AddClaimAsync(identityRole, identityClaim).Result;
        }

        public IEnumerable<ClaimModel> GetClaimsBy(RoleModel role)
        {
            var claims = _roleManager.GetClaimsAsync(new IdentityRole<int>() { Id = role.Id }).Result;

            return claims.Select(c => new ClaimModel() { Type = c.Type, Value = c.Value });
        }

        public void RemoveAssociation(int roleId, ClaimModel claim)
        {
            var identityRole = _roleManager.Roles.FirstOrDefault(r => r.Id.Equals(roleId));
            var identityClaim = new Claim(claim.Type, claim.Value);
            var identityResult = _roleManager.RemoveClaimAsync(identityRole, identityClaim).Result;
        }
    }
}
