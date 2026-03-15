using Application.Abstractions;
using Application.Customers.Dtos;

namespace Application.Customers.Queries
{
    public sealed record GetAllCustomersQuery : IQuery<IEnumerable<CustomerDto>>;
}