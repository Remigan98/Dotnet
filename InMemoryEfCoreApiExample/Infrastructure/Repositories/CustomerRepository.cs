using Application.Abstractions.Persistence;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly GroceryDbContext _dbContext;

        public CustomerRepository(GroceryDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task AddAsync(Customer customer, CancellationToken cancellationToken)
        {
            await this._dbContext.Customers.AddAsync(customer, cancellationToken);
        }

        public void Delete(Customer customer)
        {
            this._dbContext.Customers.Remove(customer);
        }

        public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await this._dbContext.Customers.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
        }

        public async Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await this._dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken)
        {
            await Task.Run(() => this._dbContext.Customers.Update(customer), cancellationToken);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await this._dbContext.Customers.ToListAsync(cancellationToken);
        }
    }
}
