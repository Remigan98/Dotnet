using Application.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Products.Commands.Delete
{
    public sealed class DeleteProductValidator : IValidator<DeleteProductCommand>
    {
        public void Validate(DeleteProductCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            if (command.ProductId <= 0)
            {
                throw new ValidationException("ProductId must be greater than zero.");
            }
        }
    }
}
