using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Common.Exceptions;
using Application.Customers.Dtos;
using Domain.Entities;

namespace Application.Customers.Commands.Create
{
    public sealed class CreateCustomerHandler : ICommandHandler<CreateCustomerCommand, CustomerDto>
    {
        private readonly ICustomerRepository _repository;
        private readonly IUnitOfWork _unitOfWork;


        public CreateCustomerHandler(ICustomerRepository repository, IUnitOfWork unitOfWork)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<CustomerDto> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            Customer customer = new Customer
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                PhoneNumber = command.PhoneNumber
            };

            Customer? shouldBeNull = await _repository.GetByEmailAsync(command.Email, cancellationToken);

            if (shouldBeNull is not null)
            {
                throw new ValidationException("A customer with the same email already exists.");
            }

            await _repository.AddAsync(customer, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CustomerDto(customer);
        }
    }
}
