using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AspNetCoreWebApiLab.Api.Models.V1;

namespace AspNetCoreWebApiLab.Api.Controllers.V1
{
    [ApiController]
    [Route("api/roles")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class RolesController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetRoles(int roleId)
        {
            try
            {
                if (roleId != 1) return NotFound("Role not found");

                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }
    }
}