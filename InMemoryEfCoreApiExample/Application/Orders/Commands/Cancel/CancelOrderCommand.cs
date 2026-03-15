using Application.Abstractions;
using Application.Orders.Dtos;

namespace Application.Orders.Commands.Cancel
{
    public sealed record CancelOrderCommand(int OrderId) : ICommand<OrderDto>;
}