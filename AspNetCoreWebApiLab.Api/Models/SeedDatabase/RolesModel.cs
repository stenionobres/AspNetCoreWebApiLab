using System.Collections.Generic;
using AspNetCoreWebApiLab.Api.Models.V1;

namespace AspNetCoreWebApiLab.Api.Models.SeedDatabase
{
    public class RolesModel
    {
        public static List<RoleModel> Roles = new List<RoleModel>()
        {
            new RoleModel() { Description = "Admin" },
            new RoleModel() { Description = "Manager" },
            new RoleModel() { Description = "Supervisor" },
            new RoleModel() { Description = "Leader" },
            new RoleModel() { Description = "Office Manager" },
            new RoleModel() { Description = "Director" },
            new RoleModel() { Description = "Strategyzer" },
            new RoleModel() { Description = "CEO" },
            new RoleModel() { Description = "CIO" },
            new RoleModel() { Description = "CFO" },
            new RoleModel() { Description = "Assistant" },
            new RoleModel() { Description = "VP" },
            new RoleModel() { Description = "Recruiter" },
            new RoleModel() { Description = "Account Executive" },
            new RoleModel() { Description = "Software Architect" },
            new RoleModel() { Description = "Head of Research" },
            new RoleModel() { Description = "Project Manager" },
            new RoleModel() { Description = "Scrum Master" },
            new RoleModel() { Description = "Project Coordinator" },
            new RoleModel() { Description = "Sales Executive" }
        };
    }
}
