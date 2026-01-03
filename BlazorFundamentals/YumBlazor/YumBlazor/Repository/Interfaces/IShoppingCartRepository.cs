using YumBlazor.Data;

namespace YumBlazor.Repository.Interfaces
{
    public interface IShoppingCartRepository
    {
        public Task<bool> UpdateCartAsync(string userId, int product, int updateBy);
        public Task<IEnumerable<ShoppingCart>> GetAllAsync(string? userId);
        public Task<bool> ClearCartsAsync(string? userId);

        public Task<int> GetTotalCartCountAsync(string? userId);
    }
}
