using Domain.Entities;
using Domain.Enums;

namespace Application.Abstractions.Persistence
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order, CancellationToken cancellationToken);
        void Delete(Order order);
        Task<Order?> GetByIdAsync(int orderId, CancellationToken cancellationToken);
        Task UpdateAsync(Order order, CancellationToken cancellationToken);
        Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
        Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken);
    }
}
