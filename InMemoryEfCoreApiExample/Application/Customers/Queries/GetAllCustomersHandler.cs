using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Customers.Dtos;
using Domain.Entities;

namespace Application.Customers.Queries
{
    public sealed class GetAllCustomersHandler : IQueryHandler<GetAllCustomersQuery, IEnumerable<CustomerDto>>
    {
        private readonly ICustomerRepository _repository;

        public GetAllCustomersHandler(ICustomerRepository repository)
        {
            this._repository = repository;
        }

        public async Task<IEnumerable<CustomerDto>> Handle(GetAllCustomersQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<Customer> customers = await _repository.GetAllAsync(cancellationToken);

            return customers.Select(c => new CustomerDto(c)).ToList();
        }
    }
}