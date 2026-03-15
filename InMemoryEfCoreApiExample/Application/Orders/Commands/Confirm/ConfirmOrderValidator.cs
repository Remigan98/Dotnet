using Application.Abstractions;
using Application.Common.Exceptions;

namespace Application.Orders.Commands.Confirm
{
    public sealed class ConfirmOrderValidator : IValidator<ConfirmOrderCommand>
    {
        public void Validate(ConfirmOrderCommand instance)
        {
            ArgumentNullException.ThrowIfNull(instance);

            if (instance.OrderId <= 0)
            {
                throw new ValidationException("Order ID must be greater than zero.");
            }
        }
    }
}