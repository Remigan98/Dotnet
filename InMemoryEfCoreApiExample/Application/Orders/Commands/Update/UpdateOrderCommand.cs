using Application.Abstractions;
using Application.Orders.Dtos;

namespace Application.Orders.Commands.Update
{
    public sealed record UpdateOrderCommand(int Id, int CustomerId, DateTime OrderDate) : ICommand<OrderDto>;
}