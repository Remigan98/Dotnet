using Application.Abstractions;
using Application.Common.Exceptions;

namespace Application.Categories.Commands.Create
{
    public sealed class CreateCategoryValidator : IValidator<CreateCategoryCommand>
    {
        public void Validate(CreateCategoryCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            if (string.IsNullOrWhiteSpace(command.Name))
            {
                throw new ValidationException("Category name is required.");
            }
        }
    }
}
