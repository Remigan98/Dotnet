using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Customers.Dtos;
using Application.Common.Exceptions;
using Domain.Entities;

namespace Application.Customers.Queries
{
    public sealed class GetCustomerByEmailHandler : IQueryHandler<GetCustomerByEmailQuery, CustomerDto>
    {
        private readonly ICustomerRepository _repository;

        public GetCustomerByEmailHandler(ICustomerRepository repository)
        {
            this._repository = repository;
        }

        public async Task<CustomerDto> Handle(GetCustomerByEmailQuery query, CancellationToken cancellationToken)
        {
            Customer? customer = await _repository.GetByEmailAsync(query.Email, cancellationToken);

            if (customer == null)
            {
                throw new NotFoundException($"Customer with email '{query.Email}' not found.");
            }

            return new CustomerDto(customer);
        }
    }
}