using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using NUnit.Framework;
using Tests.Helpers;

namespace Tests.Repositories
{
    [TestFixture]
    public class CustomerRepositoryTests
    {
        private GroceryDbContext _context = null!;
        private CustomerRepository _repository = null!;

        [SetUp]
        public void Setup()
        {
            _context = TestDbContextFactory.Create();
            _repository = new CustomerRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            TestDbContextFactory.Destroy(_context);
            _context?.Dispose();
        }

        [Test]
        public async Task AddAsync_ShouldAddCustomer()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@test.com",
                PhoneNumber = "1234567890"
            };

            // Act
            await _repository.AddAsync(customer, CancellationToken.None);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _repository.GetByIdAsync(customer.Id, CancellationToken.None);
            result.Should().NotBeNull();
            result!.FirstName.Should().Be("John");
            result.Email.Should().Be("john.doe@test.com");
        }

        [Test]
        public async Task GetByEmailAsync_ShouldReturnCustomer_WhenEmailExists()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@test.com",
                PhoneNumber = "0987654321"
            };

            await _repository.AddAsync(customer, CancellationToken.None);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByEmailAsync("jane.smith@test.com", CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.FirstName.Should().Be("Jane");
        }

        [Test]
        public async Task GetByEmailAsync_ShouldReturnNull_WhenEmailDoesNotExist()
        {
            // Act
            var result = await _repository.GetByEmailAsync("nonexistent@test.com", CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { FirstName = "John", LastName = "Doe", Email = "john@test.com", PhoneNumber = "111" },
                new Customer { FirstName = "Jane", LastName = "Smith", Email = "jane@test.com", PhoneNumber = "222" },
                new Customer { FirstName = "Bob", LastName = "Johnson", Email = "bob@test.com", PhoneNumber = "333" }
            };

            foreach (var customer in customers)
            {
                await _repository.AddAsync(customer, CancellationToken.None);
            }
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync(CancellationToken.None);

            // Assert
            result.Should().HaveCount(3);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateCustomer()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com",
                PhoneNumber = "111"
            };

            await _repository.AddAsync(customer, CancellationToken.None);
            await _context.SaveChangesAsync();

            // Act
            customer.Email = "john.updated@test.com";
            await _repository.UpdateAsync(customer, CancellationToken.None);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _repository.GetByIdAsync(customer.Id, CancellationToken.None);
            result!.Email.Should().Be("john.updated@test.com");
        }

        [Test]
        public async Task Delete_ShouldRemoveCustomer()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com",
                PhoneNumber = "111"
            };

            await _repository.AddAsync(customer, CancellationToken.None);
            await _context.SaveChangesAsync();
            var customerId = customer.Id;

            // Act
            _repository.Delete(customer);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _repository.GetByIdAsync(customerId, CancellationToken.None);
            result.Should().BeNull();
        }
    }
}