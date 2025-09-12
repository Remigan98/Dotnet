using RestaurantAPI.Entities;

namespace RestaurantAPI
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext dbContext;

        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Seed()
        {
            if (dbContext.Database.CanConnect())
            {
                if (dbContext.Restaurants.Any() == false)
                {
                    var restaurants = GetRestaurants();

                    dbContext.Restaurants.AddRange(restaurants);
                    dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            List<Restaurant> restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "KFC",
                    Category = "Fast Food",
                    Description = "Kentucky Fried Chicken (KFC) is a fast food restaurant chain that specializes in fried chicken. Founded in 1930 by Colonel Harland Sanders, KFC has become one of the largest and most recognizable fast food brands in the world.",
                    ContactEmail = "contact@kfc.com",
                    ContactNumber = "779-090-904",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Chicken Nuggets",
                            Description = "Delicious chicken nuggets made from tender chicken breast, breaded and fried to golden perfection.",
                            Price = 11.99M,
                        },
                        new Dish()
                        {
                            Name = "Chicken Wings",
                            Description = "Spicy and crispy chicken wings, seasoned with KFC's signature blend of herbs and spices.",
                            Price = 4.99M,
                        },
                    },
                    Address = new Address()
                    {
                        City = "Kraków",
                        Street = "Długa 5",
                        PostalCode = "30-001"
                    }
                },
                new Restaurant()
                {
                    Name = "McDonald's",
                    Category = "Fast Food",
                    Description = "McDonald's is a global fast food restaurant chain known for its hamburgers, fries, and breakfast items. Founded in 1940 by Richard and Maurice McDonald, the company has grown to become one of the largest and most recognizable fast food brands in the world.",
                    ContactEmail = "contact@mcdonalds.com",
                    ContactNumber = "123-456-789",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Big Mac",
                            Description = "A classic McDonald's burger featuring two beef patties, special sauce, lettuce, cheese, pickles, and onions on a sesame seed bun.",
                            Price = 14.99M,
                        },
                        new Dish()
                        {
                            Name = "Chicken McNuggets",
                            Description = "Crispy and tender chicken nuggets made from white meat chicken, served with your choice of dipping sauce.",
                            Price = 9.99M,
                        },
                    },
                    Address = new Address()
                    {
                        City = "Warszawa",
                        Street = "Marszałkowska 1",
                        PostalCode = "00-001"
                    }
                } 
            };

            return restaurants;
        }
    }
}
