using Application.Abstractions;
using Application.Common.Exceptions;

namespace Application.Orders.Commands.Create
{
    public sealed class CreateOrderValidator : IValidator<CreateOrderCommand>
    {
        public void Validate(CreateOrderCommand instance)
        {
            ArgumentNullException.ThrowIfNull(instance);

            if (instance.CustomerId <= 0)
            {
                throw new ValidationException("Customer ID must be greater than zero.");
            }

            if (instance.OrderItems == null || instance.OrderItems.Count == 0)
            {
                throw new ValidationException("Order must contain at least one item.");
            }

            foreach (var item in instance.OrderItems)
            {
                if (item.ProductId <= 0)
                {
                    throw new ValidationException("Product ID must be greater than zero.");
                }

                if (item.Quantity <= 0)
                {
                    throw new ValidationException("Quantity must be greater than zero.");
                }
            }

            List<int> duplicateProducts = instance.OrderItems
                .GroupBy(oi => oi.ProductId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateProducts.Any())
            {
                throw new ValidationException($"Duplicate products found in order: {string.Join(", ", duplicateProducts)}");
            }
        }
    }
}