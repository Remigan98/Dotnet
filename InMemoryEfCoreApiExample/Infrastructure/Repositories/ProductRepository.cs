using Application.Abstractions.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly GroceryDbContext _dbContext;

        public ProductRepository(GroceryDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken)
        {
            await this._dbContext.Products.AddAsync(product, cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await this._dbContext.Products.ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await this._dbContext.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Product product, CancellationToken cancellationToken)
        {
            await Task.Run(() => this._dbContext.Products.Update(product), cancellationToken);
        }

        public void Delete(Product product)
        {
            this._dbContext.Products.Remove(product);
        }

        public async Task<Product?> GetById(int id, CancellationToken cancellationToken)
        {
            return await this._dbContext.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetAll(CancellationToken cancellationToken)
        {
            return await this._dbContext.Products.ToListAsync(cancellationToken);
        }
    }
}
