using Application.Abstractions;
using Application.Common.Exceptions;

namespace Application.Orders.Commands.Cancel
{
    public sealed class CancelOrderValidator : IValidator<CancelOrderCommand>
    {
        public void Validate(CancelOrderCommand instance)
        {
            ArgumentNullException.ThrowIfNull(instance);

            if (instance.OrderId <= 0)
            {
                throw new ValidationException("Order ID must be greater than zero.");
            }
        }
    }
}