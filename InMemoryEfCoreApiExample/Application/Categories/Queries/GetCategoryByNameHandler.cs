using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Categories.Dtos;
using Application.Common.Exceptions;
using Domain.Entities;

namespace Application.Categories.Queries
{
    public sealed class GetCategoryByNameHandler : IQueryHandler<GetCategoryByName, CategoryDto>
    {
        private readonly ICategoryRepository _repository;

        public GetCategoryByNameHandler(ICategoryRepository repository)
        {
            this._repository = repository;
        }

        public async Task<CategoryDto> Handle(GetCategoryByName query, CancellationToken cancellationToken)
        {
            Category? category = await _repository.GetByNameAsync(query.name, cancellationToken);

            if (category == null) 
            {
                throw new NotFoundException($"Category with name '{query.name}' not found.");
            }

            return new CategoryDto
            {
                Name = category.Name,
                Description = category.Description
            };
        }
    }
}
