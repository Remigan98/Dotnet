namespace Application.Orders.Commands.Create
{
    public sealed record CreateOrderItemCommand(int ProductId, int Quantity);
}