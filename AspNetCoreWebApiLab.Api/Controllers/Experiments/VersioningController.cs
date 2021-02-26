using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWebApiLab.Api.Controllers.Experiments
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    public class VersioningController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetVersioning()
        {
            return Ok("Version 1.0");
        }

        [HttpGet]
        [MapToApiVersion("1.1")]
        public ActionResult GetVersioning11()
        {
            return Ok("Version 1.1");
        }
    }
}