using CQRS.CRUD.InMemory.Application.Products.Commands;
using CQRS.CRUD.InMemory.Domain.Entities;
using CQRS.CRUD.InMemory.Infrastructure.Persistence;
using MediatR;

namespace CQRS.CRUD.InMemory.Application.Products.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly AppDbContext _context;

        public CreateProductHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
