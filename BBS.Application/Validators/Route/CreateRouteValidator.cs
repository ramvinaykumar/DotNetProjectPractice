using BBS.Application.DTOs.Route;
using FluentValidation;

namespace BBS.Application.Validators.Route
{
    public class CreateRouteValidator  : AbstractValidator<CreateRouteRequest>
    {
        public CreateRouteValidator()
        {
            RuleFor(x => x.SourceCity)
                .NotEmpty();

            RuleFor(x => x.DestinationCity)
                .NotEmpty();

            RuleFor(x => x.DistanceKM)
                .GreaterThan(0);
        }
    }
}
