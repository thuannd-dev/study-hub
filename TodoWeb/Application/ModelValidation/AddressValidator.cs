using FluentValidation;
using TodoWeb.Application.Dtos.StudentModel;

namespace TodoWeb.Application.ModelValidation
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.Street)
                .NotEmpty()
                .WithMessage("Street is required.")
                .Length(2, 100)
                .WithMessage("Street must be between 2 and 100 characters.");
            RuleFor(x => x.ZipCode)
                .NotEmpty()
                .WithMessage("Zip code is required.")
                .Matches(@"^\d{5}(-\d{4})?$")
                .WithMessage("Invalid zip code format.");
        }
    }
}
