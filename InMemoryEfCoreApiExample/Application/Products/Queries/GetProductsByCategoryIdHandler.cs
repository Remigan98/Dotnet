using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Products.Dtos;
using Domain.Entities;

namespace Application.Products.Queries
{
    public sealed class GetProductsByCategoryIdHandler : IQueryHandler<GetProductsByCategoryIdQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository _repository;

        public GetProductsByCategoryIdHandler(IProductRepository repository)
        {
            this._repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetProductsByCategoryIdQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<Product> products = await _repository.GetAllAsync(cancellationToken);

            // Filter by CategoryId
            IEnumerable<Product> filteredProducts = products.Where(p => p.CategoryId == query.CategoryId);

            return filteredProducts.Select(p => new ProductDto(p)).ToList();
        }
    }
}