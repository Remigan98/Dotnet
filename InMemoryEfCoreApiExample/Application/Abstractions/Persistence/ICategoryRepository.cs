using Domain.Entities;

namespace Application.Abstractions.Persistence
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category, CancellationToken cancellationToken);
        Task DeleteAsync(Category category, CancellationToken cancellationToken);
        Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task UpdateAsync(Category category, CancellationToken cancellationToken);
    }
}