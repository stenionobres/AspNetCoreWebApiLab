using Microsoft.EntityFrameworkCore;
using AspNetCoreWebApiLab.Persistence.EntityFrameworkContexts;

namespace AspNetCoreWebApiLab.Persistence.Mappers
{
    public class RoleDataMapper
    {
        public void CleanRolesAndRelatedData()
        {
            using (var aspNetCoreWebApiLabDbContext = new AspNetCoreWebApiLabDbContext())
            {
                aspNetCoreWebApiLabDbContext.Database.ExecuteSqlInterpolated($"delete from AspNetRoleClaims");
                aspNetCoreWebApiLabDbContext.Database.ExecuteSqlInterpolated($"delete from AspNetUserRoles");

                aspNetCoreWebApiLabDbContext.Database.ExecuteSqlInterpolated($"delete from AspNetRoles");
                aspNetCoreWebApiLabDbContext.Database.ExecuteSqlInterpolated($"DBCC CHECKIDENT ('AspNetRoles', RESEED, 0)");
            }
        }
    }
}
