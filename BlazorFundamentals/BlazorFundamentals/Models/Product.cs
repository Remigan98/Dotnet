using System.ComponentModel.DataAnnotations;

namespace BlazorFundamentals.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; } = string.Empty;
        [Range(1, 1000)] public double Price { get; set; } = 1;
        public bool IsActive { get; set; }
        public IEnumerable<ProductProp> Properties { get; set; } = Enumerable.Empty<ProductProp>();

        public Category Category { get; set; }
        public DateOnly AvailableAfter { get; set; } = DateOnly.FromDateTime(DateTime.Now);
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
