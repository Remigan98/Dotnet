using Application.Abstractions;
using Application.Common.Exceptions;

namespace Application.Customers.Commands.Create
{
    public sealed class CreateCustomerValidator : IValidator<CreateCustomerCommand>
    {
        public void Validate(CreateCustomerCommand instance)
        {
            ArgumentNullException.ThrowIfNull(instance);

            if (string.IsNullOrWhiteSpace(instance.FirstName))
            {
                throw new ValidationException("First name is required.");
            }

            if (string.IsNullOrWhiteSpace(instance.LastName))
            {
                throw new ValidationException("Last name is required.");
            }

            if (string.IsNullOrWhiteSpace(instance.Email))
            {
                throw new ValidationException("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(instance.PhoneNumber))
            {
                throw new ValidationException("Phone number is required.");
            }
        }
    }
}
