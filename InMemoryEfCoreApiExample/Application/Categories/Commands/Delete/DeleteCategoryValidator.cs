using Application.Abstractions;
using Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Categories.Commands.Delete
{
    public sealed class DeleteCategoryValidator : IValidator<DeleteCategoryCommand>
    {
        public void Validate(DeleteCategoryCommand instance)
        {
            if (instance == null)
            {
                throw new ValidationException("Instance cannot be null");
            }

            if (instance.id <= 0)
            {
                throw new NotFoundException("id must be greater than 0");
            }
        }
    }
}
