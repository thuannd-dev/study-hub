using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TodoWeb.Constants.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace TodoWeb.Application.ActionFilters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly Role[] _userRole;

        //params ở đây là vô dụng bởi vì params chỉ dùng ở compile time
        //nhưng với DI/TypeFilter/ServiceFilter thì nó sẽ được truyền ở runtime
        public AuthorizationFilter(Role[] userRole)
        {
            _userRole = userRole;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var role = context.HttpContext.Session.GetString("Role");
            //parse sang enum, false khi không parse được - ko tồn tại role đó
            if (!Enum.TryParse<Role>(role, out var eRole))
            {
                context.Result = new StatusCodeResult(403);
                return;
            }
            if (!_userRole.Contains(eRole))
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}
