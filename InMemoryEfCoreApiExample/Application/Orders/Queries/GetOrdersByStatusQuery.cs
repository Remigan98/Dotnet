using Application.Abstractions;
using Application.Orders.Dtos;
using Domain.Enums;

namespace Application.Orders.Queries
{
    public sealed record GetOrdersByStatusQuery(OrderStatus Status) : IQuery<IEnumerable<OrderDto>>;
}