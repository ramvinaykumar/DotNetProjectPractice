using CQRS.CRUD.InMemory.Domain.Entities;
using MediatR;

namespace CQRS.CRUD.InMemory.Application.Products.Queries
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public int Id { get; set; }
    }
}
