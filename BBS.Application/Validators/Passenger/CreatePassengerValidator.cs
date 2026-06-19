using BBS.Application.Commands;
using FluentValidation;

namespace BBS.Application.Validators.Passenger
{
    /// <summary>
    /// Validates the creation of a passenger by ensuring all required fields are present and correctly formatted.
    /// </summary>
    public class CreatePassengerValidator : AbstractValidator<CreatePassengerCommand>
    {
        /// <summary>
        /// Initializes a new instance of the CreatePassengerValidator class, defining validation rules for passenger
        /// creation fields.
        /// </summary>
        public CreatePassengerValidator()
        {
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

        /// <summary>
        /// Checks if the provided date of birth indicates that the passenger is at least 18 years old. If the date of birth
        /// </summary>
        /// <param name="dob">DateTime? dob</param>
        /// <returns></returns>
        private bool BeAdult(DateTime? dob)
        {
            if (!dob.HasValue)
                return true;

            return dob.Value <= DateTime.Today.AddYears(-18);
        }
    }
}
