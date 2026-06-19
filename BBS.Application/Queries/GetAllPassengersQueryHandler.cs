using BBS.Application.DTOs.Passenger;
using BBS.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBS.Application.Queries
{
    public class GetAllPassengersQueryHandler : IRequestHandler<GetAllPassengersQuery, IEnumerable<PassengerResponse>>
    {
        private readonly IPassengerRepository _repository;

        public GetAllPassengersQueryHandler(IPassengerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PassengerResponse>> Handle(GetAllPassengersQuery request, CancellationToken cancellationToken)
        {
            var passengers = await _repository.GetAllAsync();

            return passengers.Select(x =>
                new PassengerResponse
                {
                    PassengerId = x.PassengerId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Gender = x.Gender,
                    DateOfBirth = x.DateOfBirth.ToString("dd MMM yyyy"),
                    Active = x.IsActive ? "Yes" : "No"
                });
        }
    }
}
