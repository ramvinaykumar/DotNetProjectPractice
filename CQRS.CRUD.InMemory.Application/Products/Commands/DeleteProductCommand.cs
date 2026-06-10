using MediatR;

namespace CQRS.CRUD.InMemory.Application.Products.Commands
{
    public class DeleteProductCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
