using Application.Abstractions.Persistence;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly GroceryDbContext _dbContext;

        public OrderRepository(GroceryDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task AddAsync(Order order, CancellationToken cancellationToken)
        {
            await this._dbContext.Orders.AddAsync(order, cancellationToken);
        }

        public void Delete(Order order)
        {
            this._dbContext.Orders.Remove(order);
        }

        public async Task<Order?> GetByIdAsync(int orderId, CancellationToken cancellationToken)
        {
            return await this._dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        }

        public async Task UpdateAsync(Order order, CancellationToken cancellationToken)
        {
            await Task.Run(() => this._dbContext.Orders.Update(order), cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await this._dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return await this._dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.CustomerId == customerId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken)
        {
            return await this._dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.Status == status)
                .ToListAsync(cancellationToken);
        }
    }
}
