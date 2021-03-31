using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using AspNetCoreWebApiLab.Api.Tools; 
using AspNetCoreWebApiLab.Api.Models.V1;
using AspNetCoreWebApiLab.Persistence.DataTransferObjects;

namespace AspNetCoreWebApiLab.Api.Services
{
    public class UserRoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly UserService _userService;

        public UserRoleService(UserManager<User> userManager, UserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        public void Associate(int userId, RoleModel role)
        {
            var userSaved = _userService.GetUserBy(userId);
            var identityResult = _userManager.AddToRoleAsync(userSaved, role.Description).Result;

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }

        public async Task AssociateAsync(int userId, RoleModel role)
        {
            var userSaved = await _userService.GetUserAsyncBy(userId);
            var identityResult = await _userManager.AddToRoleAsync(userSaved, role.Description);

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }

        public IEnumerable<RoleModel> GetRolesBy(UserModel user)
        {
            var roles = _userManager.GetRolesAsync(new User() { Id = user.Id }).Result;

            return roles.Select(roleName => new RoleModel() { Description = roleName });
        }

        public async Task<IEnumerable<RoleModel>> GetRolesAsyncBy(UserModel user)
        {
            var roles = await _userManager.GetRolesAsync(new User() { Id = user.Id });

            return roles.Select(roleName => new RoleModel() { Description = roleName });
        }

        public void RemoveAssociation(int userId, string roleName)
        {
            var userSaved = _userService.GetUserBy(userId);
            var identityResult = _userManager.RemoveFromRoleAsync(userSaved, roleName).Result;

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }

        public async Task RemoveAssociationAsync(int userId, string roleName)
        {
            var userSaved = await _userService.GetUserAsyncBy(userId);
            var identityResult = await _userManager.RemoveFromRoleAsync(userSaved, roleName);

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }
    }
}
