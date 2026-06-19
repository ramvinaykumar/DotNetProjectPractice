using BBS.Application.Common;
using BBS.Application.Interfaces.Repositories;
using MediatR;

namespace BBS.Application.Commands
{
    /// <summary>
    /// Handles commands to delete a passenger from the repository.
    /// </summary>
    /// <remarks>Throws a BusinessException if the specified passenger does not exist.</remarks>
    public class DeletePassengerCommandHandler : IRequestHandler<DeletePassengerCommand, bool>
    {
        private readonly IPassengerRepository _repository;

        public DeletePassengerCommandHandler(IPassengerRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeletePassengerCommand request, CancellationToken cancellationToken)
        {
            var passenger = await _repository.GetByIdAsync(request.PassengerId);

            if (passenger == null)
            {
                throw new BusinessException("Passenger not found.");
            }

            await _repository.DeleteAsync(request.PassengerId);

            return true;
        }
    }
}
