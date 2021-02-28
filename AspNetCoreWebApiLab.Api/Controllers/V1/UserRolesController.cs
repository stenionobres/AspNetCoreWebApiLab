using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AspNetCoreWebApiLab.Api.Models.V1;

namespace AspNetCoreWebApiLab.Api.Controllers.V1
{
    [ApiController]
    [Route("api/users/{id}/roles")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class UserRolesController : ControllerBase
    {
        
    }
}