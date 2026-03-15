using Application.Abstractions.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly GroceryDbContext _dbContext;

        public CategoryRepository(GroceryDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task AddAsync(Category category, CancellationToken cancellationToken)
        {
            await this._dbContext.Categories.AddAsync(category, cancellationToken);
        }

        public void Delete(Category category)
        {
            this._dbContext.Categories.Remove(category);
        }

        public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await this._dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await this._dbContext.Categories.FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
        }

        public async Task UpdateAsync(Category category, CancellationToken cancellationToken)
        {
            await Task.Run(() => this._dbContext.Categories.Update(category), cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await this._dbContext.Categories.ToListAsync(cancellationToken);
        }
    }
}
