using YumBlazor.Data;

namespace YumBlazor.Repository.Interfaces
{
    public interface IOrderRepository
    {
        public Task<OrderHeader> CreateAsync(OrderHeader orderHeader);
        public Task<OrderHeader> GetAsync(int id);
        public Task<IEnumerable<OrderHeader>> GetAllAsync(string? userId = null);
        public Task<OrderHeader> UpdateOrderStatusAsync(int orderId, string status);
    }
}
