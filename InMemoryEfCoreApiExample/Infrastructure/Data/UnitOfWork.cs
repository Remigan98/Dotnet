using Application.Abstractions;
using Application.Abstractions.Persistence;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GroceryDbContext _dbContext;

        public UnitOfWork(GroceryDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}