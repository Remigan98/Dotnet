using Application.Abstractions;

namespace Application.Products.Commands.Update
{
    public sealed class UpdateProductValidator : IValidator<UpdateProductCommand>
    {
        public void Validate(UpdateProductCommand instance)
        {
            if (instance.Id <= 0)
            {
                throw new ArgumentException("Id must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(instance.Name))
            {
                throw new ArgumentException("Name cannot be null or empty.");
            }

            if (instance.Price < 0)
            {
                throw new ArgumentException("Price cannot be negative.");
            }

            if (instance.CategoryId <= 0)
            {
                throw new ArgumentException("CategoryId must be greater than zero.");
            }

            if (instance.Name.Length > 100)
            {
                throw new ArgumentException("Name cannot exceed 100 characters.");
            }
        }
    }
}
