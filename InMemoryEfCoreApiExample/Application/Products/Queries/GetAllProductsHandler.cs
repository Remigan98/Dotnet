using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Products.Dtos;
using Domain.Entities;

namespace Application.Products.Queries
{
    public sealed class GetAllProductsHandler : IQueryHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository _repository;

        public GetAllProductsHandler(IProductRepository repository)
        {
            this._repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<Product> products = await _repository.GetAllAsync(cancellationToken);

            return products.Select(p => new ProductDto(p)).ToList();
        }
    }
}