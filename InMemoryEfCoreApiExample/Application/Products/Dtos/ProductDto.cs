using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Products.Dtos
{
    public sealed record ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }

        public ProductDto()
        {

        }

        public ProductDto(int id, string name, decimal price, int stock, int categoryId)
        {
            Id = id;
            Name = name;
            Price = price;
            Stock = stock;
            CategoryId = categoryId;
        }

        public ProductDto(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);

            Id = product.Id;
            Name = product.Name;
            Price = product.Price;
            Stock = product.Stock;
            CategoryId = product.CategoryId;
        }

        public ProductDto(ProductDto copy)
        {
            ArgumentNullException.ThrowIfNull(copy);

            Id = copy.Id;
            Name = copy.Name;
            Price = copy.Price;
            Stock = copy.Stock;
            CategoryId = copy.CategoryId;
        }
    }
}
