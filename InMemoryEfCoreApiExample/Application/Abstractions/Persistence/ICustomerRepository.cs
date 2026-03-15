using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstractions.Persistence
{
    public interface ICustomerRepository
    {   
        Task AddAsync(Customer customer, CancellationToken cancellationToken);
        void Delete(Customer customer);
        Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken);
        Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<Customer?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task UpdateAsync(Customer customer, CancellationToken cancellationToken);
    }
}
