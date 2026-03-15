using Application.Abstractions;
using Application.Orders.Dtos;

namespace Application.Orders.Commands.Confirm
{
    public sealed record ConfirmOrderCommand(int OrderId) : ICommand<OrderDto>;
}