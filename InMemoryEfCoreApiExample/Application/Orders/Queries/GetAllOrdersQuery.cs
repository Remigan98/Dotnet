using Application.Abstractions;
using Application.Orders.Dtos;

namespace Application.Orders.Queries
{
    public sealed record GetAllOrdersQuery : IQuery<IEnumerable<OrderDto>>;
}