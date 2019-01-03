using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RestApiLearn.Exceptions;

namespace RestApiLearn.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            string message = (context.Exception.InnerException ?? context.Exception).Message;
            var errorInfo = new ErrorInfo(message);
            context.Result = new JsonResult(errorInfo) { StatusCode = (int)HttpStatusCode.InternalServerError };
        }
    }
}
