using Application.Abstractions;

namespace Application.Customers.Commands.Update
{
    public sealed class UpdateCustomerValidator : IValidator<UpdateCustomerCommand>
    {
        public void Validate(UpdateCustomerCommand instance)
        {
            if (instance.Id <= 0)
            {
                throw new ArgumentException("Id must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(instance.FirstName))
            {
                throw new ArgumentException("FirstName cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(instance.LastName))
            {
                throw new ArgumentException("LastName cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(instance.Email))
            {
                throw new ArgumentException("Email cannot be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(instance.PhoneNumber))
            {
                throw new ArgumentException("PhoneNumber cannot be null or empty.");
            }

            if (instance.FirstName.Length > 100)
            {
                throw new ArgumentException("FirstName cannot exceed 100 characters.");
            }

            if (instance.LastName.Length > 100)
            {
                throw new ArgumentException("LastName cannot exceed 100 characters.");
            }

            if (instance.Email.Length > 255)
            {
                throw new ArgumentException("Email cannot exceed 255 characters.");
            }

            if (instance.PhoneNumber.Length > 20)
            {
                throw new ArgumentException("PhoneNumber cannot exceed 20 characters.");
            }
        }
    }
}