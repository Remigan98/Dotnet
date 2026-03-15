using Application.Abstractions;
using Application.Abstractions.Persistence;
using Domain.Entities;
using Application.Common.Exceptions;

namespace Application.Customers.Commands.Delete
{
    public sealed class DeleteCustomerHandler : ICommandHandler<DeleteCustomerCommand, bool>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCustomerHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            this._customerRepository = customerRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
        {
            Customer? customer = await this._customerRepository.GetByIdAsync(command.id, cancellationToken);

            if (customer is null)
            {
                throw new NotFoundException($"Customer with id {command.id} not found.");
            }

            this._customerRepository.Delete(customer);

            customer = await this._customerRepository.GetByIdAsync(command.id, cancellationToken);

            return customer is null;
        }
    }
}
