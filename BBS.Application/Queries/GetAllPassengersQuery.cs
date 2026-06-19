using BBS.Application.DTOs.Passenger;
using MediatR;

namespace BBS.Application.Queries
{
    public class GetAllPassengersQuery : IRequest<IEnumerable<PassengerResponse>>
    {

    }
}
