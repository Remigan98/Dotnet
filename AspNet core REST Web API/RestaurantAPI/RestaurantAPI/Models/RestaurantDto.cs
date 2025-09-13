namespace RestaurantAPI.Models
{
    public class RestaurantDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Category { get; set; }
        public bool HasDelivery { get; set; }
        public required string City { get; set; }
        public required string Street { get; set; }
        public required string PostalCode { get; set; }

        public List<DishDto> Dishes { get; set; } = new List<DishDto>();
    }
}