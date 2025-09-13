namespace RestaurantAPI.Entities;

public class Dish
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }

    public int RestaurantId { get; set; }
    public virtual Restaurant Restaurant { get; set; }
}
