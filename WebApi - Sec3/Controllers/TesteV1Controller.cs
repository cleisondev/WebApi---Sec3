using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi___Sec3.Controllers
{
    [Route("api/v{version:apiVersion}/teste")]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    public class TesteV1Controller : ControllerBase
    {
        [HttpGet]
        public string GetVersion()
        {
            return "TesteV1 - GET - API Version 1.0";
        }




    }
}
