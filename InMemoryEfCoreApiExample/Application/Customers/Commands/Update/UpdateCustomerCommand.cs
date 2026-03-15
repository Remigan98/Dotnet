using Application.Abstractions;
using Application.Customers.Dtos;

namespace Application.Customers.Commands.Update
{
    public sealed record UpdateCustomerCommand(int Id, string FirstName, string LastName, string Email, string PhoneNumber) : ICommand<CustomerDto>;
}