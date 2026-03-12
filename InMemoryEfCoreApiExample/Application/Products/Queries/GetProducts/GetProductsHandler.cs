using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Products.Dtos;
using Domain.Entities;

namespace Application.Products.Queries.GetProducts
{
    public sealed class GetProductsHandler : IQueryHandler<GetProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository _repository;
        public GetProductsHandler(IProductRepository repository)
        {
            this._repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery command, CancellationToken cancellationToken)
        {
            IEnumerable<Product> products = await this._repository.GetAll(cancellationToken);
            return products.Select(p => new ProductDto(p));
        }
    }
}
