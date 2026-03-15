using Domain.Entities;

namespace Application.Abstractions.Persistence
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category, CancellationToken cancellationToken);
        void Delete(Category category);
        Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task UpdateAsync(Category category, CancellationToken cancellationToken);
        Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken);
    }
}