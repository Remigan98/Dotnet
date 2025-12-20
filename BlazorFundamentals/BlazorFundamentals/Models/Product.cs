using System.ComponentModel.DataAnnotations;

namespace BlazorFundamentals.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        [Range(1,1000)] public required double Price { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<ProductProp> Properties { get; set; } = Enumerable.Empty<ProductProp>();

        public Category Category { get; set; }
        public DateOnly AvailableAfter { get; set; }
    }

    public enum Category
    {
        Electronics,
        Clothing,
        HomeGoods,
        Books,
        Toys
    }
}
