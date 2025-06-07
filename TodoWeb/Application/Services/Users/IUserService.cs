using TodoWeb.Application.Dtos.UserModel;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Application.Services.Users
{
    public interface IUserService
    {
        public Task<int> Post(UserCreateViewModel user);
        
        public User? UserLogin(UserLoginViewModel user);

        string GenerateJwt(User user);

        Task<User?> GetUserByEmail(string email);
        string GenerateRefreshToken(int userId);

        User GetUserByRefreshToken(string refreshToken);

        public void DeleteOldRefreshToken(int userId);

    }
}
