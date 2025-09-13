namespace RestaurantAPI.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public required string City { get; set; }
        public required string Street { get; set; }
        public required string PostalCode { get; set; }

        public virtual Restaurant Restaurant { get; set; }
    }
}