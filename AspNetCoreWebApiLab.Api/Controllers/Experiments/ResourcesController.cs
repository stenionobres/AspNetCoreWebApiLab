using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AspNetCoreWebApiLab.Api.Models.Experiments;

namespace AspNetCoreWebApiLab.Api.Controllers.Experiments
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ResourcesController : ControllerBase
    {
        private dynamic _resource = new { Id = 1, Description = "First resource" };

        [HttpGet]
        public ActionResult GetResource(int resourceId)
        {
            try
            {
                if (resourceId != 1) return NotFound("Resource not found");

                return Ok(_resource);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }

        [HttpPost]
        public ActionResult PostResource(ResourceModel resourceModel)
        {
            try
            {
                return Created($"/api/resources/{resourceModel.Id}", resourceModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }

        [HttpPut]
        public ActionResult PutResource(ResourceModel resourceModel)
        {
            try
            {
                if (resourceModel.Id != 1) return NotFound("Resource not found");

                return Ok(resourceModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }

        [HttpPatch]
        public ActionResult PatchResource(ResourceModel resourceModel)
        {
            try
            {
                if (resourceModel.Id != 1) return NotFound("Resource not found");

                return Ok(resourceModel);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }

        [HttpDelete]
        public ActionResult DeleteResource(int resourceId)
        {
            try
            {
                if (resourceId != 1) return NotFound("Resource not found");

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "A server error has occurred");
            }
        }
    }
}