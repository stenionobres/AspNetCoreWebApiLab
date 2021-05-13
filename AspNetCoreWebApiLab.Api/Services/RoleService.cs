using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AspNetCoreWebApiLab.Api.Tools;
using AspNetCoreWebApiLab.Api.Models.V1;
using AspNetCoreWebApiLab.Persistence.Mappers;

namespace AspNetCoreWebApiLab.Api.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly RoleDataMapper _roleDataMapper;

        public RoleService(RoleManager<IdentityRole<int>> roleManager, RoleDataMapper roleDataMapper)
        {
            _roleManager = roleManager;
            _roleDataMapper = roleDataMapper;
        }

        public RoleModel Get(int roleId)
        {
            var role = GetIdentityRoleBy(roleId);
            
            return role == null ? null : new RoleModel() { Id = role.Id, Description = role.Name };
        }

        public async Task<RoleModel> GetAsync(int roleId)
        {
            var role = await GetIdentityRoleAsyncBy(roleId);

            return role == null ? null : new RoleModel() { Id = role.Id, Description = role.Name };
        }

        public void Save(RoleModel role)
        {
            var identityRole = new IdentityRole<int>() { Name = role.Description };
            var identityResult = _roleManager.CreateAsync(identityRole).Result;
            role.Id = identityRole.Id;

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }

        public async Task SaveAsync(RoleModel role)
        {
            var identityRole = new IdentityRole<int>() { Name = role.Description };
            var identityResult = await _roleManager.CreateAsync(identityRole);
            role.Id = identityRole.Id;

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }

        public void Update(RoleModel roleSaved, RoleModel role)
        {
            var roleToBeUpdated = GetIdentityRoleBy(roleSaved.Id);
            roleToBeUpdated.Name = role.Description;

            var identityResult = _roleManager.UpdateAsync(roleToBeUpdated).Result;

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }

        public async Task UpdateAsync(RoleModel roleSaved, RoleModel role)
        {
            var roleToBeUpdated = await GetIdentityRoleAsyncBy(roleSaved.Id);
            roleToBeUpdated.Name = role.Description;

            var identityResult = await _roleManager.UpdateAsync(roleToBeUpdated);

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }

        public void Delete(RoleModel role)
        {
            var savedRole = GetIdentityRoleBy(role.Id);
            var identityResult = _roleManager.DeleteAsync(savedRole).Result;

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }

        public async Task DeleteAsync(RoleModel role)
        {
            var savedRole = await GetIdentityRoleAsyncBy(role.Id);
            var identityResult = await _roleManager.DeleteAsync(savedRole);

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }

        public IdentityRole<int> GetIdentityRoleBy(int id) => _roleManager.Roles.FirstOrDefault(r => r.Id.Equals(id));

        public async Task<IdentityRole<int>> GetIdentityRoleAsyncBy(int id) => await _roleManager.FindByIdAsync(id.ToString());

        public void CleanRolesAndRelatedData() => _roleDataMapper.CleanRolesAndRelatedData();

    }
}
