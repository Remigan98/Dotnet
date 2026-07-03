using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Customers.Dtos;
using Domain.Entities;
using Application.Common.Exceptions;

namespace Application.Customers.Commands.Update
{
    public sealed class UpdateCustomerHandler : ICommandHandler<UpdateCustomerCommand, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateCustomerCommand> _validator;

        public UpdateCustomerHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, IValidator<UpdateCustomerCommand> validator)
        {
            this._customerRepository = customerRepository;
            this._unitOfWork = unitOfWork;
            this._validator = validator;
        }

        public async Task<CustomerDto> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            _validator.Validate(command);
    
            Customer? customer = await this._customerRepository.GetByIdAsync(command.Id, cancellationToken);

            if (customer is null)
            {
                throw new NotFoundException($"Customer with id {command.Id} not found.");
            }

            customer.FirstName = command.FirstName;
            customer.LastName = command.LastName;
            customer.Email = command.Email;
            customer.PhoneNumber = command.PhoneNumber;
            customer.UpdatedAt = DateTime.UtcNow;

            await this._customerRepository.UpdateAsync(customer, cancellationToken);
            await this._unitOfWork.SaveChangesAsync(cancellationToken);

            return new CustomerDto(customer);
        }
    }
}