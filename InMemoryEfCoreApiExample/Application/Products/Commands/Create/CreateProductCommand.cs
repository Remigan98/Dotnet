using System;
using System.Collections.Generic;
using System.Text;
using Application.Abstractions;
using Application.Products.Dtos;

namespace Application.Products.Commands.Create
{
    public sealed record CreateProductCommand(string Name, decimal Price, int Stock, int CategoryId) : ICommand<ProductDto>;
}
