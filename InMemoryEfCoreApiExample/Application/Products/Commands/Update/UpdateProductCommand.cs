using Application.Abstractions;
using Application.Products.Dtos;

namespace Application.Products.Commands.Update
{
    public sealed record UpdateProductCommand(Guid Id, string Name, decimal Price, Guid CategoryId) : ICommand<ProductDto>;
}
