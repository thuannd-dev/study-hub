using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoWeb.Application.Dtos.UserModel;
using TodoWeb.Application.Helpers;
using TodoWeb.Constants.Enums;
using TodoWeb.Domains.AppsettingsConfigurations;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IValidator<UserCreateViewModel> _validator;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;
        public UserService(IValidator<UserCreateViewModel> validator, IApplicationDbContext dbContext, IMapper mapper, IOptions<JwtSettings> jwtSettingOptions)
        {
            _validator = validator;
            _dbContext = dbContext;
            _mapper = mapper;
            _jwtSettings = jwtSettingOptions.Value;
        }

        public async Task<User> Post(UserCreateViewModel user)
        {
            //var data = new Domains.Entities.User
            //{
            //    EmailAddress = user.EmailAddress,
            //    Password = user.Password,
            //    FullName = user.FullName,
            //    Role = user.Role
            //};
            var data = _mapper.Map<User>(user);
            var salting = HashHelper.GennerateRandomString(100);
            var password = user.Password + salting;
            data.Password = HashHelper.HashBcrypt(password);
            data.Salting = salting;
            await _dbContext.Users.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            return data;

        }

        public User UserLogin(UserLoginViewModel user)
        {
            //kiểm tra email, password có tồn tại trong db hay không
            var data = _dbContext.Users
                .FirstOrDefault(x => x.EmailAddress == user.EmailAddress);
            if (data == null)
            {
                return null;
            }

            var password = user.Password + data.Salting;
            if (!(data != null &&
                   HashHelper.VerifyBcrypt(password, data.Password)))
            {
                return null;
            }
            return data;
        }

        public string GenerateJwt(User user)
        {
            
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                signingCredentials: new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256Signature
                )
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<User?> GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.EmailAddress == email);
            return user;
        }

        public async Task<string> GenerateRefreshToken(int userId)
        {
            string refreshToken = HashHelper.GennerateRandomString(64);
            //refreshtoken cũng ko nên lưu dưới dạng raw data giống password,
            //nên dùng sha256 thay vì bcript vì với api tạo refresh token thì sẽ dùng nhiều hơn rất nhiều so với 
            //api login, nếu dùng bcript thì sẽ tốn thời gian hơn nhưng vẫn đc
            string hashedRefreshToken = HashHelper.Hash256(refreshToken);
            var data = new RefreshToken
            {
                UserId = userId,
                Token = hashedRefreshToken,
                ExpireTime = DateTime.UtcNow.AddDays(7), // 7 days expiration
                IsRevoked = false
            };

            await _dbContext.RefreshTokens.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            return hashedRefreshToken;

        }

        public async Task DeleteOldRefreshToken(int userId)
        {
            var entity = await _dbContext.RefreshTokens
                .Where(x => x.UserId == userId)
                .ToListAsync();
            if (entity == null)
            {
                return;
            }
            _dbContext.RefreshTokens.RemoveRange(entity);
            await _dbContext.SaveChangesAsync();
        }

        public User GetUserByRefreshToken(string refreshToken)
        {
            var user = _dbContext.RefreshTokens
                .Where(x => x.Token == refreshToken && !x.IsRevoked && x.ExpireTime > DateTime.Now)
                .Select(x => x.User)
                .FirstOrDefault();

            return user;
        }

        public async Task<User?> GetUserById(int userId)
        {
            if(userId <= 0)
            {
                return null;
            }
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }

        public async Task<User?> ChangeUserRole(int userId, Role newRole)
        {
            var user = await GetUserById(userId);
            if (user == null)
            {
                return null;
            }
            user.Role = newRole;
            //không cần phải update vì đã dùng tracker của EF Core, nếu không dùng tracker thì sẽ cần phải update
            //_dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}
