using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Common.Exceptions;
using Application.Orders.Dtos;
using Domain.Entities;

namespace Application.Orders.Queries
{
    public sealed class GetOrderByIdHandler : IQueryHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IOrderRepository _repository;

        public GetOrderByIdHandler(IOrderRepository repository)
        {
            this._repository = repository;
        }

        public async Task<OrderDto> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
        {
            Order? order = await _repository.GetByIdAsync(query.Id, cancellationToken);

            if (order == null)
            {
                throw new NotFoundException($"Order with id {query.Id} not found");
            }

            return new OrderDto(order);
        }
    }
}