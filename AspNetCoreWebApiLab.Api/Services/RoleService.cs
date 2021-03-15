﻿using System.Linq;
using Microsoft.AspNetCore.Identity;
using AspNetCoreWebApiLab.Api.Tools;
using AspNetCoreWebApiLab.Api.Models.V1;

namespace AspNetCoreWebApiLab.Api.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public RoleService(RoleManager<IdentityRole<int>> roleManager)
        {
            _roleManager = roleManager;
        }

        public RoleModel Get(int roleId)
        {
            var role = GetIdentityRoleBy(roleId);
            
            return role == null ? null : new RoleModel() { Id = role.Id, Description = role.Name };
        }

        public void Save(RoleModel role)
        {
            var identityRole = new IdentityRole<int>() { Name = role.Description };
            var identityResult = _roleManager.CreateAsync(identityRole).Result;
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

        public void Delete(RoleModel role)
        {
            var savedRole = GetIdentityRoleBy(role.Id);
            var identityResult = _roleManager.DeleteAsync(savedRole).Result;

            CustomIdentityError.CatchErrorIfNeeded(identityResult);
        }

        public IdentityRole<int> GetIdentityRoleBy(int id) => _roleManager.Roles.FirstOrDefault(r => r.Id.Equals(id));

    }
}
