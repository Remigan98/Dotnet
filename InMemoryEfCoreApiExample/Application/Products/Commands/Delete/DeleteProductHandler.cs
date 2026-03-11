using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Common.Exceptions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Products.Commands.Delete
{
    public sealed class DeleteProductHandler : ICommandHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            this._productRepository = productRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            Product? product = await this._productRepository.GetByIdAsync(command.ProductId, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException($"Product with id {command.ProductId} not found.");
            }

            await this._productRepository.RemoveAsync(product);
            await this._unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
