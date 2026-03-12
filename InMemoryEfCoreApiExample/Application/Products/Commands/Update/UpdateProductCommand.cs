using Application.Abstractions;
using Application.Products.Dtos;

namespace Application.Products.Commands.Update
{
    public sealed record UpdateProductCommand(int Id, string Name, decimal Price, int CategoryId) : ICommand<ProductDto>;
}
