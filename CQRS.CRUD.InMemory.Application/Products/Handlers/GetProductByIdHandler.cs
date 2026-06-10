using CQRS.CRUD.InMemory.Application.Products.Queries;
using CQRS.CRUD.InMemory.Domain.Entities;
using CQRS.CRUD.InMemory.Infrastructure.Persistence;
using MediatR;

namespace CQRS.CRUD.InMemory.Application.Products.Handlers
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly AppDbContext _context;

        public GetProductByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);
        }
    }
}
