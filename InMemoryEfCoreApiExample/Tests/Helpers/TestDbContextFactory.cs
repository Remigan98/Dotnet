using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Tests.Helpers
{
    public static class TestDbContextFactory
    {
        public static GroceryDbContext Create()
        {
            var options = new DbContextOptionsBuilder<GroceryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new GroceryDbContext(options);
            context.Database.EnsureCreated();

            return context;
        }

        public static void Destroy(GroceryDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}