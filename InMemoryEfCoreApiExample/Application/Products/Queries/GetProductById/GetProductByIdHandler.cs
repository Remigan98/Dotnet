using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Products.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Products.Queries.GetProductById
{
    public sealed class GetProductByIdHandler : IQueryHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IProductRepository _repository;

        public GetProductByIdHandler(IProductRepository repository)
        {
            this._repository = repository;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery command, CancellationToken cancellationToken)
        {
            Product? product = await this._repository.GetById(command.Id, cancellationToken);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with Id {command.Id} not found.");
            }

            return new ProductDto(product);
        }
    }
}
