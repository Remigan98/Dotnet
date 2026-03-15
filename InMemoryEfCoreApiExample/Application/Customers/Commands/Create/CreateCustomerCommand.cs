using Application.Abstractions;
using Application.Customers.Dtos;

namespace Application.Customers.Commands.Create
{
    public sealed record CreateCustomerCommand(string FirstName, string LastName, string Email, string PhoneNumber) : ICommand<CustomerDto>;
}
