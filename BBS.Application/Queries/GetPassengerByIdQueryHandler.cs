using BBS.Application.Common;
using BBS.Application.DTOs.Passenger;
using BBS.Application.Interfaces.Repositories;
using MediatR;

namespace BBS.Application.Queries
{
    /// <summary>
    /// Handles queries to retrieve passenger details by identifier.
    /// </summary>
    public class GetPassengerByIdQueryHandler : IRequestHandler<GetPassengerByIdQuery, PassengerResponse>
    {
        private readonly IPassengerRepository _repository;

        public GetPassengerByIdQueryHandler(IPassengerRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves a passenger by identifier asynchronously.
        /// </summary>
        /// <param name="request">The query containing the passenger identifier.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation, containing the passenger response.</returns>
        /// <exception cref="BusinessException">Thrown when the passenger is not found.</exception>
        public async Task<PassengerResponse> Handle(GetPassengerByIdQuery request, CancellationToken cancellationToken)
        {
            var passenger = await _repository.GetByIdAsync(request.PassengerId);

            if (passenger == null)
            {
                throw new BusinessException("Passenger not found.");
            }

            return new PassengerResponse
            {
                PassengerId = passenger.PassengerId,
                FirstName = passenger.FirstName,
                LastName = passenger.LastName,
                Email = passenger.Email,
                PhoneNumber = passenger.PhoneNumber,
                Gender = passenger.Gender
            };
        }
    }
}
