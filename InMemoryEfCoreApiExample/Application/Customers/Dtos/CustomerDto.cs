using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Customers.Dtos
{
    public sealed record CustomerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;

        public CustomerDto() { }

        public CustomerDto(int id, string firstName, string lastName, string email, string phoneNumber)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public CustomerDto(CustomerDto copy)
        {
            ArgumentNullException.ThrowIfNull(copy);

            Id = copy.Id;
            FirstName = copy.FirstName;
            LastName = copy.LastName;
            Email = copy.Email;
            PhoneNumber = copy.PhoneNumber;
        }

        public CustomerDto(Domain.Entities.Customer customer)
        {
            ArgumentNullException.ThrowIfNull(customer);

            Id = customer.Id;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            Email = customer.Email;
            PhoneNumber = customer.PhoneNumber;
        }
    }
}
