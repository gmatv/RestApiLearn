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
            int statusCode = (int) HttpStatusCode.InternalServerError;
            if (context.Exception is ValidationException)
            {
                statusCode = (int) HttpStatusCode.BadRequest;
            }
            context.Result = new JsonResult(errorInfo) { StatusCode = statusCode };

            _logger.LogError(statusCode, context.Exception, message);
        }
    }
}
