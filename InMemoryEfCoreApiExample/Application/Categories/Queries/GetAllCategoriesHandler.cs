using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Categories.Dtos;
using Domain.Entities;

namespace Application.Categories.Queries
{
    public sealed class GetAllCategoriesHandler : IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private readonly ICategoryRepository _repository;

        public GetAllCategoriesHandler(ICategoryRepository repository)
        {
            this._repository = repository;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<Category> categories = await _repository.GetAllAsync(cancellationToken);

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();
        }
    }
}