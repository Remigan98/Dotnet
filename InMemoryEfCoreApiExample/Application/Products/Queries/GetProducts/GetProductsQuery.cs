using Application.Abstractions;
using Application.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Products.Queries.GetProducts
{
    public sealed class GetProductsQuery() : IQuery<IEnumerable<ProductDto>>;
}
