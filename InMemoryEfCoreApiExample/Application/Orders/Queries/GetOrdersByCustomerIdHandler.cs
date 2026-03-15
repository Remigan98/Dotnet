using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Orders.Dtos;
using Domain.Entities;

namespace Application.Orders.Queries
{
    public sealed class GetOrdersByCustomerIdHandler : IQueryHandler<GetOrdersByCustomerIdQuery, IEnumerable<OrderDto>>
    {
        private readonly IOrderRepository _repository;

        public GetOrdersByCustomerIdHandler(IOrderRepository repository)
        {
            this._repository = repository;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByCustomerIdQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<Order> orders = await _repository.GetByCustomerIdAsync(query.CustomerId, cancellationToken);

            return orders.Select(o => new OrderDto(o)).ToList();
        }
    }
}