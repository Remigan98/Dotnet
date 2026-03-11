using System;
using System.Collections.Generic;
using System.Text;
using Application.Abstractions;
using Application.Common.Exceptions;

namespace Application.Products.Commands.Create
{
    public class CreateProductValidator : IValidator<CreateProductCommand>
    {
        public void Validate(CreateProductCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                throw new ValidationException("Product name is required");
            }

            if (command.Price <= 0)
            {
                throw new ValidationException("Price must be greater than zero");
            }

            if (command.Stock < 0)
            {
                throw new ValidationException("Stock cannot be negative");
            }
        }
    }
}
