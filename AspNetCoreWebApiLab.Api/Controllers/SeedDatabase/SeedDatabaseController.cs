using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AspNetCoreWebApiLab.Api.Services;
using AspNetCoreWebApiLab.Api.Models.SeedDatabase;

namespace AspNetCoreWebApiLab.Api.Controllers.SeedDatabase
{
    [ApiController]
    [Route("api/seeddatabase")]
    public class SeedDatabaseController : ControllerBase
    {
        private readonly RoleService _roleService;

        public SeedDatabaseController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        [Route("roles")]
        public IActionResult SeedRoles()
        {
            try
            {
                _roleService.CleanRolesAndRelatedData();

                foreach (var role in RolesModel.Roles)
                {
                    _roleService.Save(role);
                }

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (System.ApplicationException ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }
    }
}