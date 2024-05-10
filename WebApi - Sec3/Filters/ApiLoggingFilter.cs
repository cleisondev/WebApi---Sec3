using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi___Sec3.Filters
{
    public class ApiLoggingFilter : IActionFilter 
    {
        private readonly ILogger<ApiLoggingFilter> _logger;

        //Esse logging é integrado a plataforma .net, e permite gravar info no console.
        public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //Antes do action
            _logger.LogInformation("### Executando -> OnActionExecuting");
            _logger.LogInformation("####################################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
            _logger.LogInformation($"ModelState : {context.ModelState.IsValid}");
            _logger.LogInformation("####################################");

        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("### Executando -> OnActionExecuted");
            _logger.LogInformation("####################################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
            _logger.LogInformation($"Status code : {context.HttpContext.Response.StatusCode}");
            _logger.LogInformation("####################################");
        }

    }
}
