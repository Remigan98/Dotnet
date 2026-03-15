using Application.Abstractions;
using Application.Orders.Dtos;

namespace Application.Orders.Commands.Create
{
    public sealed record CreateOrderCommand(int CustomerId, List<CreateOrderItemCommand> OrderItems) : ICommand<OrderDto>;
}