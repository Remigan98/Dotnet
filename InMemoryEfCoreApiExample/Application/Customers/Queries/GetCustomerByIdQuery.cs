using Application.Abstractions;
using Application.Customers.Dtos;

namespace Application.Customers.Queries
{
    public sealed record GetCustomerByIdQuery(int Id) : IQuery<CustomerDto>;
}