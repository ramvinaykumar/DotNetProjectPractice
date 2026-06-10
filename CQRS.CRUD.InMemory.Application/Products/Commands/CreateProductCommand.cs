using MediatR;

namespace CQRS.CRUD.InMemory.Application.Products.Commands
{
    public class CreateProductCommand : IRequest<int>
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }
}
