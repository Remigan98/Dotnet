using Application.Abstractions;
using Application.Categories.Dtos;

namespace Application.Categories.Queries
{
    public sealed record GetAllCategoriesQuery : IQuery<IEnumerable<CategoryDto>>;
}