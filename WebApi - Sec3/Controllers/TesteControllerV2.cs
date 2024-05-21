using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi___Sec3.Controllers
{
    [Route("api/v{version:apiVersion}/teste")]
    [ApiController]
    [ApiVersion("2.0")]
    public class TesteControllerV2 : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "TesteV2 - GET - Api Versão 2.0";
        }
    }
}
