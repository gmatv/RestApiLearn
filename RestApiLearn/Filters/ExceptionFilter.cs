using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using RestApiLearn.Exceptions;

namespace RestApiLearn.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            string message = (context.Exception.InnerException ?? context.Exception).Message;
            var errorInfo = new ErrorInfo(message);
            context.Result = new JsonResult(errorInfo) { StatusCode = (int)HttpStatusCode.InternalServerError };

            _logger.LogError(500, context.Exception, message);
        }
    }
}
