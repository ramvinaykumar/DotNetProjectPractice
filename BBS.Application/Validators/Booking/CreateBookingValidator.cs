using BBS.Application.DTOs.Booking;
using FluentValidation;

namespace BBS.Application.Validators.Booking
{
    public class CreateBookingValidator : AbstractValidator<CreateBookingRequest>
    {
        public CreateBookingValidator()
        {
            RuleFor(x => x.ScheduleId)
                .GreaterThan(0);

            RuleFor(x => x.PassengerId)
                .GreaterThan(0);

            RuleFor(x => x.SeatNumber)
                .InclusiveBetween(1, 100);
        }
    }
}
