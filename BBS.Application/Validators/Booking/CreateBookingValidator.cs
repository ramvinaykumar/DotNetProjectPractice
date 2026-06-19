using BBS.Application.DTOs.Booking;
using FluentValidation;

namespace BBS.Application.Validators.Booking
{
    public class CreateBookingValidator : AbstractValidator<CreateBookingRequest>
    {
        public CreateBookingValidator()
        {
            RuleFor(x => x.PassengerId)
            .GreaterThan(0)
            .WithMessage( "Valid PassengerId is required.");

            RuleFor(x => x.ScheduleId)
                .GreaterThan(0)
                .WithMessage( "Valid ScheduleId is required.");

            RuleFor(x => x.SeatCount)
                .GreaterThan(0)
                .WithMessage( "Seat Count must be greater than zero.");

            RuleFor(x => x.SeatCount)
                .LessThanOrEqualTo(10)
                .WithMessage("Maximum 10 seats can be booked at once.");
        }
    }
}
