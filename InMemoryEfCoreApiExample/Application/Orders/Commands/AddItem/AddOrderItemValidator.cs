using Application.Abstractions;
using Application.Common.Exceptions;

namespace Application.Orders.Commands.AddItem
{
    public sealed class AddOrderItemValidator : IValidator<AddOrderItemCommand>
    {
        public void Validate(AddOrderItemCommand instance)
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

            if (instance.Quantity <= 0)
            {
                throw new ValidationException("Quantity must be greater than zero.");
            }
        }
    }
}