using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Orders.Dtos;
using Domain.Entities;

namespace Application.Orders.Queries
{
    public sealed class GetOrdersByStatusHandler : IQueryHandler<GetOrdersByStatusQuery, IEnumerable<OrderDto>>
    {
        private readonly IOrderRepository _repository;

        public GetOrdersByStatusHandler(IOrderRepository repository)
        {
            this._repository = repository;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByStatusQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<Order> orders = await _repository.GetByStatusAsync(query.Status, cancellationToken);

            return orders.Select(o => new OrderDto(o)).ToList();
        }
    }
}