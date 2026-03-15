using Application.Abstractions;
using Application.Categories.Dtos;

namespace Application.Categories.Queries
{
    public sealed record GetCategoryByIdQuery(int Id) : IQuery<CategoryDto>;
}
