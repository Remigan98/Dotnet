using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Categories.Commands.Update;
using Application.Common.Exceptions;
using Domain.Entities;

namespace Application.Categories.Dtos
{
    public sealed class UpdateCategoryHandler : ICommandHandler<UpdateCategoryCommand, CategoryDto>
    {
        private readonly ICategoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryHandler(ICategoryRepository repository, IUnitOfWork unitOfWork)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<CategoryDto> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            Category? category = await _repository.GetByIdAsync(command.Id, cancellationToken);

            if (category == null)
            {
                throw new NotFoundException($"Category with id {command.Id} not found.");
            }

            Category? nameCheckCategory = await _repository.GetByNameAsync(command.CategoryDto.Name, cancellationToken);

            if (nameCheckCategory != null) 
            {
                throw new ValidationException($"Category with name {command.CategoryDto.Name} already exists.");
            }

            category.Name = command.CategoryDto.Name;
            category.Description = command.CategoryDto.Description;

            await _repository.UpdateAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CategoryDto
            {
                Name = category.Name,
                Description = category.Description
            };
        }
    }
}
