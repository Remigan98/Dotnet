using Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Products.Commands.Delete
{
    public sealed record DeleteProductCommand(int ProductId) : ICommand<bool>;
}
