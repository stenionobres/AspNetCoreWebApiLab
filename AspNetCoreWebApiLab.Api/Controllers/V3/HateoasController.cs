using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AspNetCoreWebApiLab.Api.Models.V3;

namespace AspNetCoreWebApiLab.Api.Controllers.V3
{
    [ApiController]
    [ApiVersion("3.0")]
    [Route("api/v{version:apiVersion}")]
    [ApiExplorerSettings(GroupName = "IdentityAPI-V3.0")]
    public class HateoasController : ControllerBase
    {
        [HttpGet(Name = "ApiEndpoints")]
        public IActionResult GetApiEndpoints()
        {
            var endpoints = new List<Endpoint>();
            var host = Request.Host;
            var version = "v3";

            endpoints.Add(new Endpoint($"{host}/api/{version}", "self", "GET"));
            endpoints.Add(new Endpoint($"{host}/api/{version}/users", "create_user", "POST"));
            endpoints.Add(new Endpoint($"{host}/api/{version}/users/signin", "signin_user", "POST"));

            return Ok(endpoints);
        }
    }
}