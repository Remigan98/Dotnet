using Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Customers.Commands.Delete
{
    public sealed record DeleteCustomerCommand(int id) : ICommand<bool>;
}
