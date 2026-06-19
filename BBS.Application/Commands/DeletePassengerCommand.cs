using MediatR;

namespace BBS.Application.Commands
{
    public class DeletePassengerCommand : IRequest<bool>
    {
        public int PassengerId { get; set; }
    }
}
