using TodoWeb.Application.Dtos.UserModel;
using TodoWeb.Constants.Enums;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Application.Services.Users
{
    public interface IUserService
    {
        public Task<User> Post(UserCreateViewModel user);
        
        public User? UserLogin(UserLoginViewModel user);

        string GenerateJwt(User user);

        Task<User?> GetUserById(int userId);

        Task<User?> GetUserByEmail(string email);
        Task<string> GenerateRefreshToken(int userId);

        User GetUserByRefreshToken(string refreshToken);

        public Task DeleteOldRefreshToken(int userId);

        public Task<User?> ChangeUserRole(int userId, Role newRole);

    }
}
