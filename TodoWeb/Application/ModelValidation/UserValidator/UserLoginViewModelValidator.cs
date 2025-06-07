using FluentValidation;
using TodoWeb.Application.Dtos.UserModel;

namespace TodoWeb.Application.ModelValidation.UserValidator
{
    public class UserLoginViewModelValidator : AbstractValidator<UserLoginViewModel>
    {
        public UserLoginViewModelValidator()
        {
            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .WithMessage("Email address is required.")
                .EmailAddress()
                .WithMessage("Invalid email address format.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .Length(6, 100)
                .WithMessage("Password must be between 6 and 100 characters long.");
        }
    }
}
