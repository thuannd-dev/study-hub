using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using TodoWeb.Application.Services.CacheService;

namespace TodoWeb.Application.ActionFilters
{
    public class CacheFilter : ActionFilterAttribute
    {
        public readonly int _duration;
        //private readonly ICacheService _cacheService; //mặc định sẽ cache ở trong DI container chứ không cần phải truyền vào 
        //để trước hay sau duration đều đc
        private readonly IMemoryCache _cache;
        public CacheFilter(int duration, IMemoryCache cache)//, ICacheService cacheService)
        {
            _duration = duration;
            //_cacheService = cacheService;
            _cache = cache;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var cacheKey = context.HttpContext.Request.Path.ToString();
            //var cacheData = _cacheService.Get(cacheKey);
            var cacheData = _cache.Get(cacheKey);
            if (cacheData != null)
            {
                context.Result = new OkObjectResult(cacheData);
                //context.Result sẽ ngắt luồng request và trả về kết quả luôn => ko cần return nữa
            }
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var cacheKey = context.HttpContext.Request.Path.ToString();
            var result = context.Result as OkObjectResult;
            if (result != null)
            {
                //_cacheService.Set(cacheKey, result.Value!, _duration);
                _cache.Set(cacheKey, result.Value!, TimeSpan.FromSeconds(_duration));
            }
        }

    }
}
