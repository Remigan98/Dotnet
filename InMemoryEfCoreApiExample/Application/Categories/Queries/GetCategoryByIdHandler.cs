using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Categories.Dtos;
using Application.Common.Exceptions;
using Domain.Entities;

namespace Application.Categories.Queries
{
    public sealed class GetCategoryByIdHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
    {
        private readonly ICategoryRepository _repository;

        public GetCategoryByIdHandler(ICategoryRepository repository)
        {
            this._repository = repository;
        }

        public async Task<CategoryDto> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
        {
            Category? category = await _repository.GetByIdAsync(query.Id, cancellationToken);

            if (category == null) 
            {
                throw new NotFoundException($"Category with id {query.Id} not found");
            }

            return new CategoryDto
            {
                Name = category.Name,
                Description = category.Description
            };
        }
    }
}
