using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Orders.Dtos;
using Domain.Entities;

namespace Application.Orders.Queries
{
    public sealed class GetAllOrdersHandler : IQueryHandler<GetAllOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IOrderRepository _repository;

        public GetAllOrdersHandler(IOrderRepository repository)
        {
            this._repository = repository;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetAllOrdersQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<Order> orders = await _repository.GetAllAsync(cancellationToken);

            return orders.Select(o => new OrderDto(o)).ToList();
        }
    }
}