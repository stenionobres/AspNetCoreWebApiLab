using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using AspNetCoreWebApiLab.Api.Tools;
using AspNetCoreWebApiLab.Api.Models.V1;
using AspNetCoreWebApiLab.Persistence.DataTransferObjects;

namespace AspNetCoreWebApiLab.Api.Services
{
    public class UserClaimService
    {
        private readonly UserManager<User> _userManager;
        private readonly UserService _userService;

        public UserClaimService(UserManager<User> userManager, UserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        public void Associate(int userId, ClaimModel claim)
        {
            var userSaved = _userService.GetUserBy(userId);
            var identityClaim = new Claim(claim.Type, claim.Value);
            var identityResult = _userManager.AddClaimAsync(userSaved, identityClaim).Result;

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }

        public async Task AssociateAsync(int userId, ClaimModel claim)
        {
            var userSaved = await _userService.GetUserAsyncBy(userId);
            var identityClaim = new Claim(claim.Type, claim.Value);
            var identityResult = await _userManager.AddClaimAsync(userSaved, identityClaim);

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }

        public IEnumerable<ClaimModel> GetClaimsBy(UserModel user)
        {
            var identityUser = _userService.GetUserBy(user.Id);
            var claims = _userManager.GetClaimsAsync(identityUser).Result;

            return claims.Select(c => new ClaimModel() { Type = c.Type, Value = c.Value });
        }

        public void RemoveAssociation(int userId, ClaimModel claim)
        {
            var userSaved = _userService.GetUserBy(userId);
            var identityClaim = new Claim(claim.Type, claim.Value);
            var identityResult = _userManager.RemoveClaimAsync(userSaved, identityClaim).Result;

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }
    }
}
