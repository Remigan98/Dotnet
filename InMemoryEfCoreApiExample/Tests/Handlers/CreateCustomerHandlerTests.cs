using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Common.Exceptions;
using Application.Customers.Commands.Create;
using Application.Customers.Dtos;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using NUnit.Framework;
using Tests.Helpers;

namespace Tests.Handlers
{
    [TestFixture]
    public class CreateCustomerHandlerTests
    {
        private GroceryDbContext _context = null!;
        private ICustomerRepository _repository = null!;
        private IUnitOfWork _unitOfWork = null!;
        private CreateCustomerHandler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _context = TestDbContextFactory.Create();
            _repository = new CustomerRepository(_context);
            _unitOfWork = new UnitOfWork(_context);
            _handler = new CreateCustomerHandler(_repository, _unitOfWork);
        }

        [TearDown]
        public void TearDown()
        {
            TestDbContextFactory.Destroy(_context);
            _context?.Dispose();
        }

        [Test]
        public async Task Handle_ShouldCreateCustomer_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateCustomerCommand(
                "John",
                "Doe",
                "john.doe@test.com",
                "1234567890"
            );

            // Act
            CustomerDto result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.FirstName.Should().Be("John");
            result.Email.Should().Be("john.doe@test.com");

            // Verify in database
            var customer = await _repository.GetByIdAsync(result.Id, CancellationToken.None);
            customer.Should().NotBeNull();
        }

        [Test]
        public void Handle_ShouldThrowValidationException_WhenEmailAlreadyExists()
        {
            // Arrange
            var email = "duplicate@test.com";
            var command1 = new CreateCustomerCommand("John", "Doe", email, "1234567890");
            var command2 = new CreateCustomerCommand("Jane", "Smith", email, "0987654321");

            // Act
            _handler.Handle(command1, CancellationToken.None).Wait();

            // Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
                await _handler.Handle(command2, CancellationToken.None));
        }
    }
}