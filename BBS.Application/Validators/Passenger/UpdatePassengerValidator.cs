using BBS.Application.Commands;
using FluentValidation;

namespace BBS.Application.Validators.Passenger
{
    /// <summary>
    /// Validates the properties of an UpdatePassengerCommand to ensure they meet specified criteria.
    /// </summary>
    public class UpdatePassengerValidator : AbstractValidator<UpdatePassengerCommand>
    {
        /// <summary>
        /// Initializes a new instance of the UpdatePassengerValidator class with rules to validate passenger update
        /// requests.
        /// </summary>
        public UpdatePassengerValidator()
        {
            RuleFor(x => x.PassengerId)
                .GreaterThan(0);

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First Name is required.")
                .MaximumLength(100)
                .WithMessage("First Name cannot exceed 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last Name is required.")
                .MaximumLength(100)
                .WithMessage("Last Name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Please enter a valid email address.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("PhoneNumber is required.")
                .Matches(@"^[0-9]{10}$")
                .WithMessage("Phone Number must contain exactly 10 digits.");

            RuleFor(x => x.Gender)
                .NotEmpty()
                .WithMessage("Gender is required.")
                .Must(x =>
                    x == "Male" ||
                    x == "Female" ||
                    x == "Other")
                .WithMessage("Gender must be Male, Female or Other.");

            RuleFor(x => x.DateOfBirth)
                .NotNull()
                .WithMessage("DateOfBirth is required.")
                .Must(BeAdult)
                .WithMessage("Passenger must be at least 18 years old.");
        }

        private bool BeAdult(DateTime? dob)
        {
            if (!dob.HasValue)
                return true;
            return dob.Value <= DateTime.Today.AddYears(-18);
        }
    }
}
