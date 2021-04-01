using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AspNetCoreWebApiLab.Api.Models.V1;
using AspNetCoreWebApiLab.Api.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreWebApiLab.Api.Controllers.V2
{
    [ApiController]
    [Authorize]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/roles")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "IdentityAPI-V2.0")]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _roleService;
        private readonly RoleClaimService _roleClaimService;

        public RolesController(RoleService roleService, RoleClaimService roleClaimService)
        {
            _roleService = roleService;
            _roleClaimService = roleClaimService;
        }

        [HttpGet("{roleId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetRoles(int roleId)
        {
            try
            {
                var role = await _roleService.GetAsync(roleId);

                if (role == null) return NotFound("Role not found");

                return Ok(role);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RoleModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PostRoles(RoleModel role)
        {
            try
            {
                await _roleService.SaveAsync(role);

                return Created($"/api/roles/{role.Id}", role);
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

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PutRoles(RoleModel role)
        {
            try
            {
                var roleSaved = await _roleService.GetAsync(role.Id);

                if (roleSaved == null) return NotFound("Role not found");

                await _roleService.UpdateAsync(roleSaved, role);

                return Ok(role);
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

        [HttpDelete("{roleId}")]
        [Authorize(Roles = "Admin, CanDeleteRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteRoles(int roleId)
        {
            try
            {
                var role = await _roleService.GetAsync(roleId);

                if (role == null) return NotFound("Role not found");

                await _roleService.DeleteAsync(role);

                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }

        [HttpOptions]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult OptionsRoles()
        {
            Response.Headers.Add("Allow", "GET,POST,PUT,DELETE");
            return Ok();
        }

        /// <summary>
        /// Associates a claim with a role. If claim doesn't exists it's created with new id.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{roleId}/claims")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ClaimModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PostRoleClaims(int roleId, ClaimModel claim)
        {
            try
            {
                var role = await _roleService.GetAsync(roleId);

                if (role == null) return NotFound("Role not found");

                await _roleClaimService.AssociateAsync(role.Id, claim);

                return Created($"/api/roles/{roleId}/claims", claim);
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

        /// <summary>
        /// Get claims associated with role.
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{roleId}/claims")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ClaimModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetRoleClaims(int roleId)
        {
            try
            {
                var role = await _roleService.GetAsync(roleId);

                if (role == null) return NotFound("Role not found");

                var claims = await _roleClaimService.GetClaimsAsyncBy(role);

                return Ok(claims);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }

        /// <summary>
        /// Removes claim associated with role.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{roleId}/claims")]
        [Authorize(Roles = "Admin, CanDeleteRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteRoleClaims(int roleId, ClaimModel claim)
        {
            try
            {
                await _roleClaimService.RemoveAssociationAsync(roleId, claim);

                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }
    }
}