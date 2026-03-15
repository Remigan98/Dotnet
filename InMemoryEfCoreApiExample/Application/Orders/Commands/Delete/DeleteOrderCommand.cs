using Application.Abstractions;

namespace Application.Orders.Commands.Delete
{
    public sealed record DeleteOrderCommand(int Id) : ICommand<bool>;
}