using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AspNetCoreWebApiLab.Api.Services;
using AspNetCoreWebApiLab.Api.Models.V1;
using AspNetCoreWebApiLab.Api.Models.SeedDatabase;

namespace AspNetCoreWebApiLab.Api.Controllers.SeedDatabase
{
    [ApiController]
    [Route("api/seeddatabase")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SeedDatabaseController : ControllerBase
    {
        private readonly RoleService _roleService;
        private readonly UserService _userService;
        private readonly UserRoleService _userRoleService;

        public SeedDatabaseController(RoleService roleService, UserService userService, UserRoleService userRoleService)
        {
            _roleService = roleService;
            _userService = userService;
            _userRoleService = userRoleService;
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

        [HttpPost]
        [Route("users")]
        public IActionResult SeedUsers()
        {
            try
            {
                _userService.CleanUsersAndRelatedData();

                foreach (var user in UsersModel.Users)
                {
                    _userService.Save(user);
                }

                _userRoleService.Associate(userId: 1, new RoleModel() { Id = 1, Description = "Admin" });

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