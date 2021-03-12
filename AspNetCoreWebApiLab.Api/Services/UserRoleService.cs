using AspNetCoreWebApiLab.Api.Models.V1;
using System.Collections.Generic;

namespace AspNetCoreWebApiLab.Api.Services
{
    public class UserRoleService
    {
        private readonly RoleService _roleService;

        public UserRoleService(RoleService roleService)
        {
            _roleService = roleService;
        }

        public void Associate(UserModel user, RoleModel role)
        {
            var roleSaved = _roleService.Get(role.Id);

            if (roleSaved == null)
            {
                _roleService.Save(role);
                roleSaved = role;
            }
        }

        public IEnumerable<RoleModel> GetRolesBy(UserModel user)
        {
            return new List<RoleModel>();
        }

        public void RemoveAssociation(int userId, int roleId)
        {

        }
    }
}
