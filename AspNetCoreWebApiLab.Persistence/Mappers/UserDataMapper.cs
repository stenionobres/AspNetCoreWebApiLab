using Microsoft.EntityFrameworkCore;
using AspNetCoreWebApiLab.Persistence.EntityFrameworkContexts;

namespace AspNetCoreWebApiLab.Persistence.Mappers
{
    public class UserDataMapper
    {
        public void CleanUsersAndRelatedData()
        {
            using (var aspNetCoreWebApiLabDbContext = new AspNetCoreWebApiLabDbContext())
            {
                aspNetCoreWebApiLabDbContext.Database.ExecuteSqlInterpolated($"delete from AspNetUserClaims");

                aspNetCoreWebApiLabDbContext.Database.ExecuteSqlInterpolated($"delete from AspNetUsers");
                aspNetCoreWebApiLabDbContext.Database.ExecuteSqlInterpolated($"DBCC CHECKIDENT ('AspNetUsers', RESEED, 0)");
            }
        }
    }
}
