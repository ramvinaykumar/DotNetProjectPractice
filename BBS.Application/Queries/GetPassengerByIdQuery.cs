using BBS.Application.DTOs.Passenger;
using MediatR;

namespace BBS.Application.Queries
{
    public class GetPassengerByIdQuery : IRequest<PassengerResponse>
    {
        public int PassengerId { get; set; }
    }
}
