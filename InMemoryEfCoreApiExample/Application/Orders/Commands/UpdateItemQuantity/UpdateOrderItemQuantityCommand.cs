using Application.Abstractions;
using Application.Orders.Dtos;

namespace Application.Orders.Commands.UpdateItemQuantity
{
    public sealed record UpdateOrderItemQuantityCommand(int OrderId, int ProductId, int NewQuantity) : ICommand<OrderDto>;
}