using BBS.Application.DTOs.Bus;
using FluentValidation;

namespace BBS.Application.Validators.Bus
{
    public class CreateBusValidator : AbstractValidator<CreateBusRequest>
    {
        public CreateBusValidator()
        {
            RuleFor(x => x.BusNumber)
                .NotEmpty();

            RuleFor(x => x.BusName)
                .NotEmpty();

            RuleFor(x => x.TotalSeats)
                .GreaterThan(0);
        }
    }
}
