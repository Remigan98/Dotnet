using Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Exceptions;

namespace Application.Customers.Commands.Delete
{
    public sealed class DeleteCustomerValidator : IValidator<DeleteCustomerCommand>
    {
        public void Validate(DeleteCustomerCommand instance)
        {
            if (instance.id < 0)
            {
                throw new ValidationException("Customer ID must be a non-negative integer.");
            }
        }
    }
}
