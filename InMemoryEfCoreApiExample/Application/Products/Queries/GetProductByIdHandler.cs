using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Common.Exceptions;
using Application.Products.Dtos;
using Domain.Entities;

namespace Application.Products.Queries
{
    public sealed class GetProductByIdHandler : IQueryHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IProductRepository _repository;

        public GetProductByIdHandler(IProductRepository repository)
        {
            this._repository = repository;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            Product? product = await _repository.GetByIdAsync(query.Id, cancellationToken);

            if (product == null)
            {
                throw new NotFoundException($"Product with id {query.Id} not found");
            }

            return new ProductDto(product);
        }
    }
}