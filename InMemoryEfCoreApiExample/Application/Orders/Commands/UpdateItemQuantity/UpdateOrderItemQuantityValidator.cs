using Application.Abstractions;
using Application.Common.Exceptions;

namespace Application.Orders.Commands.UpdateItemQuantity
{
    public sealed class UpdateOrderItemQuantityValidator : IValidator<UpdateOrderItemQuantityCommand>
    {
        public void Validate(UpdateOrderItemQuantityCommand instance)
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

            if (instance.NewQuantity <= 0)
            {
                throw new ValidationException("New quantity must be greater than zero.");
            }
        }
    }
}