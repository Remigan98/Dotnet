using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Products.Dtos;
using Domain.Entities;

namespace Application.Products.Commands.Create
{
    public sealed class CreateProductHandler : ICommandHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _products;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductHandler(IProductRepository products, IUnitOfWork unitOfWork)
        {
            this._products = products;
            this._unitOfWork = unitOfWork;
        }


        public async Task<ProductDto> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            Product product = new Product
            {
                Name = command.Name,
                Price = command.Price,
                Stock = command.Stock,
                CategoryId = command.CategoryId
            };

            await this._products.AddAsync(product, cancellationToken);
            await this._unitOfWork.SaveChangesAsync(cancellationToken);

            return new ProductDto(product);
        }
    }
}
