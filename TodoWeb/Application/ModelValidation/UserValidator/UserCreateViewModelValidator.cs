using FluentValidation;
using TodoWeb.Application.Dtos.UserModel;
using TodoWeb.Constants.Enums;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.ModelValidation.UserValidator
{
    public class UserCreateViewModelValidator : AbstractValidator<UserCreateViewModel>
    {
        private readonly IApplicationDbContext _dbContext;

        public UserCreateViewModelValidator(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Full name is required.")
                .Length(2, 50)
                .WithMessage("Full name must be between 2 and 50 characters.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .Length(6, 100)
                .WithMessage("Password must be between 6 and 100 characters long.");

            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.")
                .Must(DoesNotExistEmail)
                .WithMessage("Email already exists.");

            RuleFor(x => x.Role)
                .IsInEnum()
                .WithMessage("Role must be a valid enum value.");
        }

        private bool DoesNotExistEmail(string emailAddress)
        {
            return !_dbContext.Users.Any(s => s.EmailAddress == emailAddress);
        }
    }

}
