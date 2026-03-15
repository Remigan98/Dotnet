using Application.Abstractions;
using Application.Orders.Dtos;

namespace Application.Orders.Commands.RemoveItem
{
    public sealed record RemoveOrderItemCommand(int OrderId, int ProductId) : ICommand<OrderDto>;
}