using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Products.Dtos;
using Domain.Entities;
using Application.Common.Exceptions;

namespace Application.Products.Commands.Update
{
    public sealed class UpdateProductHandler : ICommandHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductHandler(IProductRepository repository, IUnitOfWork unitOfWork)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<ProductDto> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            Product? product = await this._repository.GetByIdAsync(command.Id, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException($"Product with id {command.Id} not found.");
            }

            product.Name = command.Name;
            product.Price = command.Price;
            product.CategoryId = command.CategoryId;
            product.UpdatedAt = DateTime.UtcNow;

            await this._repository.UpdateAsync(product, cancellationToken);
            await this._unitOfWork.SaveChangesAsync(cancellationToken);

            return new ProductDto(product);
        }
    }
}
