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
    }
}
