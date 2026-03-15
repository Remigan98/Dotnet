using Application.Abstractions;
using Application.Products.Dtos;

namespace Application.Products.Queries
{
    public sealed record GetProductsByCategoryIdQuery(int CategoryId) : IQuery<IEnumerable<ProductDto>>;
}