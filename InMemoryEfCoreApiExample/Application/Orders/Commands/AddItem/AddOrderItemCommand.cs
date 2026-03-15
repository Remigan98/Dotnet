using Application.Abstractions;
using Application.Orders.Dtos;

namespace Application.Orders.Commands.AddItem
{
    public sealed record AddOrderItemCommand(int OrderId, int ProductId, int Quantity) : ICommand<OrderDto>;
}