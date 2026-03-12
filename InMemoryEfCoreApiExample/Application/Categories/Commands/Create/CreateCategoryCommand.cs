using Application.Abstractions;
using Application.Categories.Dtos;

namespace Application.Categories.Commands.Create
{
    public sealed record CreateCategoryCommand(string Name, string Description) : ICommand<CategoryDto>;
}
