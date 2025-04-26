
namespace TodoWeb.Application.Middleware
{
    //trong 30s hệ thống chỉ cho phép 10 request
    public class RateLimitMiddleware : IMiddleware
    {
        private readonly ILogger<RateLimitMiddleware> _logger;
        private static int _requestCount = 0;
        private static DateTime _startTime;
        public RateLimitMiddleware(ILogger<RateLimitMiddleware> logger)
        {
            _logger = logger;
            _startTime = DateTime.Now;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var currentTime = DateTime.Now;
            var timeElapsed = (currentTime - _startTime).TotalSeconds;
            if (timeElapsed > 30)
            {
                _requestCount = 0;
                _startTime = currentTime;
            }
            if (_requestCount >= 10)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
                return;
                //return tức là trả về respone cho user luôn mà không cần tới end point, hay middleware nào nữa, kết thúc hàm luôn
            }
            _requestCount++;
            await next(context);
        }

    }
}
