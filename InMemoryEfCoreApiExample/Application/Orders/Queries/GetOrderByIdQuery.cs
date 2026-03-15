using Application.Abstractions;
using Application.Orders.Dtos;

namespace Application.Orders.Queries
{
    public sealed record GetOrderByIdQuery(int Id) : IQuery<OrderDto>;
}