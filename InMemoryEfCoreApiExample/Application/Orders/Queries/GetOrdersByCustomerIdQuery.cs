using Application.Abstractions;
using Application.Orders.Dtos;

namespace Application.Orders.Queries
{
    public sealed record GetOrdersByCustomerIdQuery(int CustomerId) : IQuery<IEnumerable<OrderDto>>;
}