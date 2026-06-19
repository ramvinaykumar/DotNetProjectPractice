using BBS.Application.Common;
using BBS.Application.DTOs.Passenger;
using BBS.Application.Interfaces.Repositories;
using BBS.Domain.Entities;
using MediatR;

namespace BBS.Application.Commands
{
    /// <summary>
    /// Handles creation and deletion of passenger records in the system.
    /// </summary>
    /// <remarks>Validates passenger data and interacts with the passenger repository to ensure data integrity
    /// and enforce business rules.</remarks>
    public class CreatePassengerCommandHandler : IRequestHandler<CreatePassengerCommand, PassengerResponse>
    {
        private readonly IPassengerRepository _repository;

        public CreatePassengerCommandHandler(IPassengerRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Insert a passenger's details and returns the inserted information.
        /// </summary>
        /// <param name="request">The command containing create passenger details.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A response with the create passenger information.</returns>
        /// <exception cref="BusinessException">Thrown if the specified passenger does not exist.</exception>
        public async Task<PassengerResponse> Handle(CreatePassengerCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.EmailExistsAsync(request.Email))
            {
                throw new BusinessException("Email already exists.");
            }

            if (await _repository.PhoneExistsAsync(request.PhoneNumber))
            {
                throw new BusinessException("Phone Number already exists.");
            }

            var passenger = new Passenger
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth.GetValueOrDefault(),
                IsActive = true
            };

            var passengerId = await _repository.CreateAsync(passenger);
            return await GetDataById(passengerId);
        }

        /// <summary>
        /// Retrieves passenger details by unique identifier.
        /// </summary>
        /// <param name="passengerId">The unique identifier of the passenger.</param>
        /// <returns>A PassengerResponse containing the passenger's information.</returns>
        /// <exception cref="BusinessException">Thrown when the passenger is not found.</exception>
        private async Task<PassengerResponse> GetDataById(int passengerId)
        {
            var createdPassenger = await _repository.GetByIdAsync(passengerId);

            if (createdPassenger == null)
            {
                throw new BusinessException("Failed to create passenger.");
            }

            return new PassengerResponse
            {
                PassengerId = createdPassenger.PassengerId,
                FirstName = createdPassenger.FirstName,
                LastName = createdPassenger.LastName,
                Email = createdPassenger.Email,
                PhoneNumber = createdPassenger.PhoneNumber,
                Gender = createdPassenger.Gender
            };
        }
    }
}
