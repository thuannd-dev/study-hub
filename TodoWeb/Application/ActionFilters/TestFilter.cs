using Microsoft.AspNetCore.Mvc.Filters;

namespace TodoWeb.Application.ActionFilters
{
    public class TestFilter : IActionFilter, IResultFilter
    {

        public void OnActionExecuting(ActionExecutingContext context) 
        {
            Console.WriteLine("OnActionExecuting");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("OnActionExcuted");
        }
        //Onresult chỉ tác dụng với response, ko có request
        //trước khi chuyển thành byte
        public void OnResultExecuting(ResultExecutingContext context)
        {
            Console.WriteLine("OnResultExecuting");
        }
        //sau khi chuyển thành byte
        public void OnResultExecuted(ResultExecutedContext context)
        {
            Console.WriteLine("OnResultExecuted");
        }
    }
}
