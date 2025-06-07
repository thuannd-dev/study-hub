using FluentValidation;
using TodoWeb.Application.Dtos.StudentModel;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.ModelValidation
{
    public class StudentViewModelValidator : AbstractValidator<StudentViewModel>
    {
        private readonly IApplicationDbContext _dbContext;

        public StudentViewModelValidator(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Full name is required.")
                .Length(2, 50)
                .WithMessage("Full name must be between 2 and 50 characters.")
                .Must(DoesNotExist)
                .WithMessage("Full name already exists.");

            RuleFor(x => x.Age)
                .InclusiveBetween(1, 120)
                .WithMessage("Age must be between 1 and 120.");

            RuleFor(x => x.SchoolName)
                .NotEmpty()
                .WithMessage("School name is required.")
                .Length(2, 100)
                .WithMessage("School name must be between 2 and 100 characters.");

            RuleFor(x => x.Balance)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Balance must be a positive number.");

            RuleForEach(x => x.Emails)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");

            RuleFor(x => x.Address)
                .SetValidator(new AddressValidator())
                .WithMessage("Invalid address format.");

        }
        
        private bool DoesNotExist(string fullName)
        {
            return !_dbContext.Students.Any(s => s.FirstName + " " + s.LastName == fullName);
        }

    }
}
