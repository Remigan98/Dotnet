using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Customers.Dtos;
using Application.Common.Exceptions;
using Domain.Entities;

namespace Application.Customers.Queries
{
    public sealed class GetCustomerByIdHandler : IQueryHandler<GetCustomerByIdQuery, CustomerDto>
    {
        private readonly ICustomerRepository _repository;

        public GetCustomerByIdHandler(ICustomerRepository repository)
        {
            this._repository = repository;
        }

        public async Task<CustomerDto> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
        {
            Customer? customer = await _repository.GetByIdAsync(query.Id, cancellationToken);

            if (customer == null)
            {
                throw new NotFoundException($"Customer with id {query.Id} not found");
            }

            return new CustomerDto(customer);
        }
    }
}