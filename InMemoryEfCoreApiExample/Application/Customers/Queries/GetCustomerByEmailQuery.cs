using Application.Abstractions;
using Application.Customers.Dtos;

namespace Application.Customers.Queries
{
    public sealed record GetCustomerByEmailQuery(string Email) : IQuery<CustomerDto>;
}