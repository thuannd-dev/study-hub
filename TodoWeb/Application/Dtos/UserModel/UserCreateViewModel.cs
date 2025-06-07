using TodoWeb.Constants.Enums;

namespace TodoWeb.Application.Dtos.UserModel
{
    public class UserCreateViewModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public Role Role { get; set; }
    }
}
