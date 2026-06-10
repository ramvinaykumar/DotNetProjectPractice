using CQRS.CRUD.InMemory.Application.Products.Commands;
using CQRS.CRUD.InMemory.Infrastructure.Persistence;
using MediatR;

namespace CQRS.CRUD.InMemory.Application.Products.Handlers
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly AppDbContext _context;

        public UpdateProductHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with Id {request.Id} not found.");
            }

            product.Name = request.Name;
            product.Price = request.Price;

            _context.Products.Update(product);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
