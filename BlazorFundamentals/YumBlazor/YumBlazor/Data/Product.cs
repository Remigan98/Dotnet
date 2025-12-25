using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YumBlazor.Data
{
    public class Product
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; } = string.Empty;
        [Range(0.01, 1000)] public decimal Price { get; set; } = decimal.Zero;
        public string? Description { get; set; }
        public string? Tag { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")] public Category Category { get; set; }

        public string? ImageUrl { get; set; }
    }
}
