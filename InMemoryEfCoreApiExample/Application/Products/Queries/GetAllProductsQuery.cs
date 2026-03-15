using Application.Abstractions;
using Application.Products.Dtos;

namespace Application.Products.Queries
{
    public sealed record GetAllProductsQuery : IQuery<IEnumerable<ProductDto>>;
}