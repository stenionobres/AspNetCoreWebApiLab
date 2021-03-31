using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using AspNetCoreWebApiLab.Api.Tools;
using AspNetCoreWebApiLab.Api.Models.V1;

namespace AspNetCoreWebApiLab.Api.Services
{
    public class RoleClaimService
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly RoleService _roleService;

        public RoleClaimService(RoleManager<IdentityRole<int>> roleManager, RoleService roleService)
        {
            _roleManager = roleManager;
            _roleService = roleService;
        }

        public void Associate(int roleId, ClaimModel claim)
        {
            var identityRole = _roleService.GetIdentityRoleBy(roleId);
            var identityClaim = new Claim(claim.Type, claim.Value);
            var identityResult = _roleManager.AddClaimAsync(identityRole, identityClaim).Result;

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }

        public async Task AssociateAsync(int roleId, ClaimModel claim)
        {
            var identityRole = await _roleService.GetIdentityRoleAsyncBy(roleId);
            var identityClaim = new Claim(claim.Type, claim.Value);
            var identityResult = await _roleManager.AddClaimAsync(identityRole, identityClaim);

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }

        public IEnumerable<ClaimModel> GetClaimsBy(RoleModel role)
        {
            var claims = _roleManager.GetClaimsAsync(new IdentityRole<int>() { Id = role.Id }).Result;

            return claims.Select(c => new ClaimModel() { Type = c.Type, Value = c.Value });
        }

        public void RemoveAssociation(int roleId, ClaimModel claim)
        {
            var identityRole = _roleService.GetIdentityRoleBy(roleId);
            var identityClaim = new Claim(claim.Type, claim.Value);
            var identityResult = _roleManager.RemoveClaimAsync(identityRole, identityClaim).Result;

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }
    }
}
