using BBS.Application.DTOs.Schedule;
using FluentValidation;

namespace BBS.Application.Validators.Schedule
{
    public class CreateScheduleValidator : AbstractValidator<CreateScheduleRequest>
    {
        public CreateScheduleValidator()
        {
            RuleFor(x => x.BusId)
                .GreaterThan(0);

            RuleFor(x => x.RouteId)
                .GreaterThan(0);

            RuleFor(x => x.Fare)
                .GreaterThan(0);

            RuleFor(x => x.ArrivalTime)
                .GreaterThan(x => x.DepartureTime)
                .WithMessage("Arrival Time must be greater than Departure Time");
        }
    }
}
