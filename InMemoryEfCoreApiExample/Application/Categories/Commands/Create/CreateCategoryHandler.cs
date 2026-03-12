using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Categories.Dtos;
using Application.Common.Exceptions;
using Domain.Entities;

namespace Application.Categories.Commands.Create
{
    public sealed class CreateCategoryHandler : ICommandHandler<CreateCategoryCommand, CategoryDto>
    {
        private readonly ICategoryRepository _categories;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryHandler(ICategoryRepository categories, IUnitOfWork unitOfWork)
        {
            this._categories = categories;
            this._unitOfWork = unitOfWork;
        }

        public async Task<CategoryDto> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            Category? category = await this._categories.GetByNameAsync(command.Name, cancellationToken);

            if (category is not null)
            {
                throw new ValidationException($"Category with name '{command.Name}' already exists.");
            }

            category = new Category
            {
                Name = command.Name,
                Description = command.Description
            };

            await this._categories.AddAsync(category, cancellationToken);
            await this._unitOfWork.SaveChangesAsync();

            return new CategoryDto(category);
        }
    }
}
