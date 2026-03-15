using Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Categories.Commands.Update
{
    public sealed class UpdateCategoryValidator : IValidator<UpdateCategoryCommand>
    {
        public void Validate(UpdateCategoryCommand instance)
        {
            ArgumentNullException.ThrowIfNull(instance);

            if (instance.Id <= 0)
            {
                throw new ArgumentException("Id must be greater than zero.", nameof(instance.Id));
            }
            if (instance.CategoryDto is null)
            {
                throw new ArgumentNullException(nameof(instance.CategoryDto), "CategoryDto cannot be null.");
            }
             if (string.IsNullOrWhiteSpace(instance.CategoryDto.Name))
            {
                throw new ArgumentException("Name cannot be null or whitespace.", nameof(instance.CategoryDto.Name));
            }
        }
    }
}
