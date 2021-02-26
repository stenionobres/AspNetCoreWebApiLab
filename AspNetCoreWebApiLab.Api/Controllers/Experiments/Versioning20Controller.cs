using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWebApiLab.Api.Controllers.Experiments
{
    [Route("api/v{version:apiVersion}/versioning")]
    [ApiController]
    [ApiVersion("2.0")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class Versioning20Controller : ControllerBase
    {
        [HttpGet]
        public ActionResult GetVersioning()
        {
            return Ok("Version 2.0");
        }
    }
}