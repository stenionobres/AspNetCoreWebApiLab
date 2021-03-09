using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreWebApiLab.Api.Models.V1;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;

namespace AspNetCoreWebApiLab.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/users")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "IdentityAPI-V1.0")]
    public class UsersController : ControllerBase
    {
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetUsers(int userId)
        {
            try
            {
                if (userId != 1) return NotFound("User not found");

                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }

        [HttpGet("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult GetUsers(string username)
        {
            try
            {
                if (username != "example@example.com") return NotFound("User not found");

                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult PostUsers(UserModel user)
        {
            try
            {
                return Created($"/api/users/{user.Id}", user);
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult PutUsers(UserModel user)
        {
            try
            {
                if (user.Id != 1) return NotFound("User not found");

                return Ok(user);
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
                var userModel = new UserModel();

                userModelPatchDocument.ApplyTo(userModel);

                return Ok(userModel);
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
                if (userId != 1) return NotFound("User not found");

                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }

        [HttpDelete("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteUsers(string username)
        {
            try
            {
                if (username != "example@example.com") return NotFound("User not found");

                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }

        /// <summary>
        /// Associates an user with a role. 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{userId}/roles")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RoleModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult PostUserRoles(int userId, RoleModel role)
        {
            try
            {
                if (userId != 1) return NotFound("User not found");

                return Created($"/api/users/{userId}/roles", role);
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
                if (userId != 1) return NotFound("User not found");

                return Ok();
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
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{userId}/roles/{roleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteUserRoles(int userId, int roleId)
        {
            try
            {
                if (userId != 1) return NotFound("User not found");

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

    }
}