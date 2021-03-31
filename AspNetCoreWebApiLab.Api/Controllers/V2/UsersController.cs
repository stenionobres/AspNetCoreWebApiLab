using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreWebApiLab.Api.Models.V1;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using AspNetCoreWebApiLab.Api.Services;

namespace AspNetCoreWebApiLab.Api.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/users")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "IdentityAPI-V2.0")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly UserRoleService _userRoleService;
        private readonly UserClaimService _userClaimService;

        public UsersController(UserService userService, UserRoleService userRoleService, UserClaimService userClaimService)
        {
            _userService = userService;
            _userRoleService = userRoleService;
            _userClaimService = userClaimService;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetUsers(int userId)
        {
            try
            {
                var user = await _userService.GetAsync(userId);

                if (user == null) return NotFound("User not found");

                return Ok(user);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PostUsers(UserPostModel user)
        {
            try
            {
                var userCreated = await _userService.SaveAsync(user);

                return Created($"/api/users/{user.Id}", userCreated);
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult PutUsers(UserModel user)
        {
            try
            {
                var userSaved = _userService.Get(user.Id);

                if (userSaved == null) return NotFound("User not found");

                _userService.Update(userSaved.Id, user);

                return Ok(user);
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

        [HttpPatch("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult PatchUsers(int userId, JsonPatchDocument<UserModel> userModelPatchDocument)
        {
            try
            {
                var user = _userService.Get(userId);

                if (user == null) return NotFound("User not found");

                userModelPatchDocument.ApplyTo(user);

                _userService.Update(userId, user);

                return Ok(user);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteUsers(int userId)
        {
            try
            {
                var user = _userService.Get(userId);

                if (user == null) return NotFound("User not found");

                _userService.Delete(user);

                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }

        [HttpOptions]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult OptionsUsers()
        {
            Response.Headers.Add("Allow", "GET,POST,PUT,PATCH,DELETE");
            return Ok();
        }

        /// <summary>
        /// Associates an user with a role. If role doesn't exists it's created with new id.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{userId}/roles")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RoleModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult PostUserRoles(int userId, RoleModel role)
        {
            try
            {
                var user = _userService.Get(userId);

                if (user == null) return NotFound("User not found");

                _userRoleService.Associate(userId, role);

                return Created($"/api/users/{userId}/roles", role);
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
        /// Get roles associated with user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId}/roles")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RoleModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetUserRoles(int userId)
        {
            try
            {
                var user = _userService.Get(userId);

                if (user == null) return NotFound("User not found");

                var roles = _userRoleService.GetRolesBy(user);

                return Ok(roles);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }

        /// <summary>
        /// Removes role associated with user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{userId}/roles/{roleName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteUserRoles(int userId, string roleName)
        {
            try
            {
                _userRoleService.RemoveAssociation(userId, roleName);

                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }

        /// <summary>
        /// Associates an user with a claim. If claim doesn't exists it's created with new id.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{userId}/claims")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ClaimModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult PostUserClaims(int userId, ClaimModel claim)
        {
            try
            {
                var user = _userService.Get(userId);

                if (user == null) return NotFound("User not found");

                _userClaimService.Associate(userId, claim);

                return Created($"/api/users/{userId}/claims", claim);
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
        /// Get claims associated with user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId}/claims")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ClaimModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetUserClaims(int userId)
        {
            try
            {
                var user = _userService.Get(userId);

                if (user == null) return NotFound("User not found");

                var claims = _userClaimService.GetClaimsBy(user);

                return Ok(claims);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }

        /// <summary>
        /// Removes claims associated with user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{userId}/claims")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteUserClaims(int userId, ClaimModel claim)
        {
            try
            {
                _userClaimService.RemoveAssociation(userId, claim);

                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }

        
    }
}