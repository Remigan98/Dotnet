using Microsoft.EntityFrameworkCore;
using YumBlazor.Data;
using YumBlazor.Repository.Interfaces;

namespace YumBlazor.Repository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        ApplicationDbContext dbContext;

        public ShoppingCartRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> ClearCartsAsync(string? userId)
        {
            var cartItems = dbContext.ShoppingCarts.Where(u => u.UserId == userId);
            dbContext.ShoppingCarts.RemoveRange(cartItems);

            return await dbContext.SaveChangesAsync() > 0;
        }

        public  async Task<IEnumerable<ShoppingCart>> GetAllAsync(string? userId)
        {
            return await dbContext.ShoppingCarts.Where(u => u.UserId == userId).Include(u => u.Product).ToListAsync();
        }

        public async Task<bool> UpdateCartAsync(string userId, int product, int updateBy)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return await Task.FromResult(false);
            }

            var cart = dbContext.ShoppingCarts.FirstOrDefault(u => u.UserId == userId && u.ProductId == product);

            if (cart == null)
            {
                ShoppingCart newCart = new()
                {
                    UserId = userId,
                    ProductId = product,
                    Count = updateBy
                };

                await dbContext.ShoppingCarts.AddAsync(newCart);
            }
            else
            {
                cart.Count += updateBy;

                if (cart.Count <= 0)
                {
                    dbContext.ShoppingCarts.Remove(cart);
                }
                else
                {
                    dbContext.ShoppingCarts.Update(cart);
                }
            }

            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<int> GetTotalCartCountAsync(string? userId)
        {
            var count = await dbContext.ShoppingCarts.Where(u => u.UserId == userId).SumAsync(u => u.Count);
            return count;
        }
    }
}
