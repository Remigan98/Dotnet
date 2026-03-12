using Application.Abstractions;
using Application.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Products.Queries.GetProductById
{
    public sealed record GetProductByIdQuery(int Id) : IQuery<ProductDto>;
}
