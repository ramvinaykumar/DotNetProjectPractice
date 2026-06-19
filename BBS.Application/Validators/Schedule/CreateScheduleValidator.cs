using BBS.Application.DTOs.Schedule;
using FluentValidation;

namespace BBS.Application.Validators.Schedule
{
    public class CreateScheduleValidator : AbstractValidator<CreateScheduleRequest>
    {
        public CreateScheduleValidator()
        {
            RuleFor(x => x.BusId)
            .GreaterThan(0)
            .WithMessage("Valid Bus Id is required.");

            RuleFor(x => x.RouteId)
                .GreaterThan(0)
                .WithMessage("Valid Route Id is required.");

            RuleFor(x => x.Fare)
                .GreaterThan(0)
                .WithMessage("Fare must be greater than zero.");

            RuleFor(x => x.Fare)
                .LessThanOrEqualTo(10000)
                .WithMessage("Fare cannot exceed 10000.");

            RuleFor(x => x.DepartureTime)
                .NotEmpty()
                .WithMessage("Departure Time is required.");

            RuleFor(x => x.ArrivalTime)
                .NotEmpty()
                .WithMessage("Arrival Time is required.");

            RuleFor(x => x.DepartureTime)
                .GreaterThan(DateTime.Now)
                .WithMessage("Departure Time must be in the future.");

            RuleFor(x => x.ArrivalTime)
                .GreaterThan(x => x.DepartureTime)
                .WithMessage("Arrival Time must be greater than Departure Time.");

            RuleFor(x => x)
                .Must(HaveValidTripDuration)
                .WithMessage("Trip duration cannot exceed 48 hours.");
        }

        private bool HaveValidTripDuration(CreateScheduleRequest request)
        {
            var duration = request.ArrivalTime - request.DepartureTime;
            return duration.TotalHours <= 48;
        }
    }
}
