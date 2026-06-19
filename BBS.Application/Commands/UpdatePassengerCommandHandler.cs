using BBS.Application.Common;
using BBS.Application.DTOs.Passenger;
using BBS.Application.Interfaces.Repositories;
using MediatR;

namespace BBS.Application.Commands
{
    /// <summary>
    /// Handles update commands for passenger entities by modifying their details in the repository.
    /// </summary>
    public class UpdatePassengerCommandHandler : IRequestHandler<UpdatePassengerCommand, PassengerResponse>
    {
        private readonly IPassengerRepository _repository;

        public UpdatePassengerCommandHandler(IPassengerRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Updates a passenger's details and returns the updated information.
        /// </summary>
        /// <param name="request">The command containing updated passenger details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A response with the updated passenger information.</returns>
        /// <exception cref="BusinessException">Thrown if the specified passenger does not exist.</exception>
        public async Task<PassengerResponse> Handle(UpdatePassengerCommand request, CancellationToken cancellationToken)
        {
            var passenger = await _repository.GetByIdAsync(request.PassengerId);

            if (passenger == null)
            {
                throw new BusinessException("Passenger not found.");
            }

            passenger.FirstName = request.FirstName;
            passenger.LastName = request.LastName;
            passenger.Email = request.Email;
            passenger.PhoneNumber = request.PhoneNumber;
            passenger.Gender = request.Gender;
            passenger.DateOfBirth = request.DateOfBirth.GetValueOrDefault();

            await _repository.UpdateAsync(passenger);

            return new PassengerResponse
            {
                PassengerId = passenger.PassengerId,
                FirstName = passenger.FirstName,
                LastName = passenger.LastName,
                Email = passenger.Email,
                PhoneNumber = passenger.PhoneNumber
            };
        }
    }
}
