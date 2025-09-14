using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class UpdateRestaurantDto
    {
        [MaxLength(25)]
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required bool HasDelivery { get; set; }
    }
}
