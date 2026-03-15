using Application.Abstractions;
using Application.Common.Exceptions;

namespace Application.Orders.Commands.RemoveItem
{
    public sealed class RemoveOrderItemValidator : IValidator<RemoveOrderItemCommand>
    {
        public void Validate(RemoveOrderItemCommand instance)
        {
            ArgumentNullException.ThrowIfNull(instance);

            if (instance.OrderId <= 0)
            {
                throw new ValidationException("Order ID must be greater than zero.");
            }

            if (instance.ProductId <= 0)
            {
                throw new ValidationException("Product ID must be greater than zero.");
            }
        }
    }
}