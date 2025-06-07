using TodoWeb.Constants.Enums;

namespace TodoWeb.Domains.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string Salting { get; set; }
        public string FullName { get; set; }
        public Role Role { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
