using Microsoft.EntityFrameworkCore;
using YumBlazor.Data;
using YumBlazor.Repository.Interfaces;

namespace YumBlazor.Repository
{
    public class OrderRepository : IOrderRepository
    {
        readonly ApplicationDbContext dbContext;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<OrderHeader> CreateAsync(OrderHeader orderHeader)
        {
            orderHeader.OrderDate = DateTime.Now;

            await dbContext.OrderHeaders.AddAsync(orderHeader);
            await dbContext.SaveChangesAsync();

            return orderHeader;
        }

        public async Task<OrderHeader> GetAsync(int id)
        {
            return await dbContext.OrderHeaders.Include(u => u.OrderDetails).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<OrderHeader>> GetAllAsync(string? userId = null)
        {
            if (string.IsNullOrEmpty(userId) == false)
            {
                return await dbContext.OrderHeaders.Where(u => u.UserId == userId).ToListAsync();
            }

            return await dbContext.OrderHeaders.ToListAsync();
        }

        public async Task<OrderHeader> UpdateOrderStatusAsync(int orderId, string status)
        {
            var orderHeader = await dbContext.OrderHeaders.FirstOrDefaultAsync(u => u.Id == orderId);

            if (orderHeader != null)
            {
                orderHeader.Status = status;
                await dbContext.SaveChangesAsync();
            }

            return orderHeader;
        }
    }
}
