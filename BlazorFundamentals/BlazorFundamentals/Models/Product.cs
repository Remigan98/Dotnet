namespace BlazorFundamentals.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<ProductProp> Properties { get; set; } = Enumerable.Empty<ProductProp>();
    }
}
