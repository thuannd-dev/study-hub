using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TodoWeb.Application.Dtos.UserModel;
using TodoWeb.Application.Dtos.UserModel.Contracts;
using TodoWeb.Application.Services.Users;
using TodoWeb.Application.Services.Users.FacebookService;
using TodoWeb.Application.Services.Users.GoogleService;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IGoogleCredentialService _googleCredentialService;
        private readonly IFacebookCredentialService _facebookCredentialService;
        private readonly IMemoryCache _cache;
        public UserController(IUserService userService, IGoogleCredentialService googleCredentialService, IFacebookCredentialService facebookCredentialService, IMemoryCache cache)
        {
            _userService = userService;
            _googleCredentialService = googleCredentialService;
            _facebookCredentialService = facebookCredentialService;
            _cache = cache;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Register(UserCreateViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _userService.Post(user));

        }

        [HttpPost("Login")]
        public IActionResult Login(UserLoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = _userService.UserLogin(loginViewModel);
            if (user == null)
            {
                return NotFound("Username or password is wrong");
            }
            //kiểm tra xem user có bị chặn không, nếu bị chặn thì không cho đăng nhập
            if (user.Role == Constants.Enums.Role.Banned)
            {
                return Forbid("Your account has been revoked. Please contact admin to support.");
            }
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Role", user.Role.ToString());
            //{key: session, value: {UserId, Role}}
            return Ok("Login success");

        }

        [HttpPost("login-cookies")]
        public async Task<IActionResult> LoginCookies(UserLoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userService.UserLogin(loginViewModel);
            if (user == null)
            {
                return NotFound("Username or password is wrong");
            }
            //kiểm tra xem user có bị chặn không, nếu bị chặn thì không cho đăng nhập
            if (user.Role == Constants.Enums.Role.Banned)
            {
                return Forbid("Your account has been revoked. Please contact admin to support.");
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
            return Ok("Login successfully!");
        }

        [HttpPost("login-google")]
        public async Task<IActionResult> LoginGoogle(GoogleLoginModel model)
        {
            //ToDO: get from appsettings.json

            //var clientId = "983687873578-rs5o6560m6n73uviuv1vug8sns5imh7v.apps.googleusercontent.com";
            //var payload = await _googleCredentialService.VerifyCredential(clientId, model.Credential);
            try
            {
                var payload = await _googleCredentialService.VerifyCredential(model.Credential);
                // Check if the payload is null or not
                if (payload == null)
                {
                    return BadRequest("Invalid Google credential.");
                }
                //kiểm tra xem email đã tồn tại trong db chưa, nếu chưa thì tạo mới user,
                //có rồi thì chỉ cần cấp access token và refresh token mới
                var existingUser = await _userService.GetUserByEmail(payload.Email);
                if (existingUser != null && existingUser.Role == Constants.Enums.Role.Banned)
                {
                    return Forbid("Your account has been revoked. Please contact admin to support.");
                }
                if(existingUser == null){
                    existingUser = await _userService.Post(new UserCreateViewModel
                    {
                        EmailAddress = payload.Email,
                        FullName = payload.Name,
                        Password = "",
                        Role = Constants.Enums.Role.User
                    });
                }

                //ToDo: Generate JWT token or set session
                //delete the old refresh token if it exists
                await _userService.DeleteOldRefreshToken(existingUser.Id);
                var accesstoken = _userService.GenerateJwt(existingUser);
                var refreshToken = await _userService.GenerateRefreshToken(existingUser.Id);
                //set refresh token in cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = false,
                    Secure = false, // Use Secure cookies in production
                    Expires = DateTime.UtcNow.AddDays(30), // Set expiration for the cookie
                };
                HttpContext.Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
                //Use an anonymous object instead of named parameters for 'object'
                return Ok(new { Token = accesstoken, Payload = payload, RefreshToken = refreshToken });
            }
            catch
            {
                return BadRequest("Google credential verification failed.");
            }

        }

        [HttpPost("login-facebook")]
        public async Task<IActionResult> LoginFacebook(FacebookLoginModel model)
        {

            try
            {
                var validationTokenResult = await _facebookCredentialService.ValidateAccessTokenAsync(model.AccessToken);
                if (!validationTokenResult.Data.IsValid)
                {
                    return BadRequest("Invalid Facebook access token.");
                }
                //nếu token hợp lệ thì lấy thông tin người dùng
                var userInfo = await _facebookCredentialService.GetUserInfoAsync(model.AccessToken);
                if (userInfo == null)
                {
                    return BadRequest("Failed to retrieve user information from Facebook.");
                }
                //kiểm tra xem email đã tồn tại trong db chưa, nếu chưa thì tạo mới user,
                //có rồi thì chỉ cần cấp access token và refresh token mới
                var existingUser = await _userService.GetUserByEmail(userInfo.Email);
                if (existingUser != null && existingUser.Role == Constants.Enums.Role.Banned)
                {
                    return Forbid("Your account has been revoked. Please contact admin to support.");
                    
                }
                if (existingUser == null)
                {
                    existingUser = await _userService.Post(new UserCreateViewModel
                    {
                        EmailAddress = userInfo.Email,
                        FullName = userInfo.Name,
                        Password = "",
                        Role = Constants.Enums.Role.User
                    });
                }

                //ToDo: Generate access token and refresh token
                //delete the old refresh token if it exists
                await _userService.DeleteOldRefreshToken(existingUser.Id);
                //generate new access token and refresh token
                var accessToken = _userService.GenerateJwt(existingUser);
                var refreshToken = await _userService.GenerateRefreshToken(existingUser.Id);
                //set refresh token in cookie
                var cookieOptions = new CookieOptions
                {
                    //HttpOnly = true,
                    //tậm thời command lại do front  đang dùng http
                    //Secure = true, // Use Secure cookies in production
                    Expires = DateTime.UtcNow.AddDays(30), // Set expiration for the cookie
                };
                HttpContext.Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
                //Use an anonymous object instead of named parameters for 'object'
                return Ok(new { Token = accessToken, UserInfo = userInfo, RefreshToken = refreshToken });
            }
            catch
            {
                return BadRequest("Google credential verification failed.");
            }
            
        }
    

        [HttpPost("login-jwt")]
        public async Task<IActionResult> LoginJwt(UserLoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userService.UserLogin(loginViewModel);
            if (user == null)
            {
                return NotFound("Username or password is wrong");
            }
            //kiểm tra xem user có bị chặn không, nếu bị chặn thì không cho đăng nhập
            if (user.Role == Constants.Enums.Role.Banned)
            {
                return Forbid("Your account has been revoked. Please contact admin to support.");
            }
            //Delete the old refresh token if it exists
            await _userService.DeleteOldRefreshToken(user.Id);
            // Generate JWT token and refresh token
            var accessToken = _userService.GenerateJwt(user);
            var refreshToken = await _userService.GenerateRefreshToken(user.Id);
            //Set the refresh token in the session or cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Use Secure cookies in production
                Expires = DateTime.UtcNow.AddDays(30), // Set expiration for the cookie
            };
            HttpContext.Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
            return Ok(accessToken);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            //lấy refresh token từ cookie
            var isExist = HttpContext.Request.Cookies.TryGetValue("RefreshToken", out var refreshToken);
            if (!isExist || refreshToken == null || refreshToken.Length == 0)
            {
                return Unauthorized("Refresh token not found.");
            }
            var user = _userService.GetUserByRefreshToken(refreshToken!);
            if (user == null)
            {
                return Unauthorized("Invalid refresh token.");
            }
            // Delete the old refresh token
            await _userService.DeleteOldRefreshToken(user.Id);

            // Generate new access token and refresh token
            var accessToken = _userService.GenerateJwt(user);
            var newRefreshToken = await _userService.GenerateRefreshToken(user.Id);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Use Secure cookies in production
                Expires = DateTime.UtcNow.AddDays(30), // Set expiration for the cookie
            };

            HttpContext.Response.Cookies.Append("RefreshToken", newRefreshToken, cookieOptions);
            return Ok(accessToken);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(int userId)
        {
            //xóa access token ở cookie
            //phía client sẽ xóa access token
            // Delete the refresh token from the database
            await _userService.DeleteOldRefreshToken(userId);
            //xóa refresh token ở cookie
            HttpContext.Response.Cookies.Delete("RefreshToken");
            // Clear session
            HttpContext.Session.Clear();
            //xóa refresh token ở db
            return Ok();
        }

        // Cache build in .Net, Time to live
        // Set user to InActive
        // Delete Refresh Token
        // User still keep access token, assume expire time is 15 minutes

        // BAN user's access token
        // Create a revoke check middleware, cache contains revoke userid with key "REVOKE_USER:{userId}"
        // middleware check in the request pipeline has the userId 
        //if userId is null, continue processing
        // if has userId continue check if user's access token exist in the cache
        // if has  => return forbidden status code 403 and message "Your account has been revoked. Please contact admin to support."
        // No => continue processing
        
        [HttpPost("Revoke")]
        public async Task<IActionResult> Revoke(int userId)
        {
            //chặn ng dùng không cho truy cập vào hệ thống nữa, sau 10 phút thì có thể đăng nhập lại bình thường
            //set role user to InActive-Banned
            var user = await _userService.ChangeUserRole(userId, Constants.Enums.Role.Banned);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            // Clear session and cookies 
            HttpContext.Session.Clear();
            // Delete the refresh token from the database
            await _userService.DeleteOldRefreshToken(userId);
            // Delete the refresh token cookie
            HttpContext.Response.Cookies.Delete("RefreshToken");
            // Add user to cache with key "REVOKE_USER:{userId}" and value true
            _cache.Set($"REVOKE_USER:{userId}", true, TimeSpan.FromMinutes(10));//accesstoken expries in 1 minutes
            return Ok("User has been revoked successfully.");
        }
    }
}
