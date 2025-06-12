using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;

namespace TodoWeb.Application.Middleware
{
    public class RevokeCheckMiddleware : IMiddleware
    {
        private readonly ILogger<RevokeCheckMiddleware> _logger;
        private readonly IMemoryCache _cache;
        public RevokeCheckMiddleware(ILogger<RevokeCheckMiddleware> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //await next(context);
            //var userId2 = context.User.Claims.ElementAtOrDefault(0)?.Value;
            //var userId3 = context.User.Claims.FirstOrDefault()?.Value;
            var userId = context.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                await next(context);
                return;
            }
            if (_cache.TryGetValue($"REVOKE_USER:{userId}", out var cacheUser))
            {
                _logger.LogWarning($"User {userId} is revoked. Request will be blocked. The request at {DateTime.Now}");
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("Your account has been revoked. Please contact admin to support.");
                return;
            }
            await next(context);
        }
    }
}
