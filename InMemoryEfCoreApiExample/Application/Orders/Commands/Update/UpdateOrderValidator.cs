using Application.Abstractions;
using Application.Common.Exceptions;

namespace Application.Orders.Commands.Update
{
    public sealed class UpdateOrderValidator : IValidator<UpdateOrderCommand>
    {
        public void Validate(UpdateOrderCommand instance)
        {
            ArgumentNullException.ThrowIfNull(instance);

            if (instance.Id <= 0)
            {
                throw new ValidationException("Id must be greater than zero.");
            }

            if (instance.CustomerId <= 0)
            {
                throw new ValidationException("CustomerId must be greater than zero.");
            }

            if (instance.OrderDate == default)
            {
                throw new ValidationException("OrderDate cannot be default.");
            }

            if (instance.OrderDate > DateTime.UtcNow)
            {
                throw new ValidationException("OrderDate cannot be in the future.");
            }
        }
    }
}