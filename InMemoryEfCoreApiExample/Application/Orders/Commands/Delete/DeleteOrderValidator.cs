using Application.Abstractions;
using Application.Common.Exceptions;

namespace Application.Orders.Commands.Delete
{
    public sealed class DeleteOrderValidator : IValidator<DeleteOrderCommand>
    {
        public void Validate(DeleteOrderCommand instance)
        {
            if (instance.Id < 0)
            {
                throw new ValidationException("Order ID must be a non-negative integer.");
            }
        }
    }
}