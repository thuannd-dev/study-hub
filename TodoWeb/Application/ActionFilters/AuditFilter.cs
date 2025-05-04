using Microsoft.AspNetCore.Mvc.Filters;

namespace TodoWeb.Application.ActionFilters
{
    public class AuditFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var method = request.Method;
            var path = request.Path;
            var argument = context.ActionArguments;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var result =  context.Result;
            var statusCode = context.HttpContext.Response.StatusCode;

        }
    }
}
