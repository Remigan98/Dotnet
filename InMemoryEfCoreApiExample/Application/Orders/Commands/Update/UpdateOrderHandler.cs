using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Common.Exceptions;
using Application.Orders.Dtos;
using Domain.Entities;

namespace Application.Orders.Commands.Update
{
    public sealed class UpdateOrderHandler : ICommandHandler<UpdateOrderCommand, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderHandler(IOrderRepository orderRepository, ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._customerRepository = customerRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<OrderDto> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            Order? order = await _orderRepository.GetByIdAsync(command.Id, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException($"Order with id {command.Id} not found.");
            }

            Customer? customer = await _customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);

            if (customer is null)
            {
                throw new NotFoundException($"Customer with id {command.CustomerId} not found.");
            }

            order.CustomerId = command.CustomerId;
            order.OrderDate = command.OrderDate;
            order.UpdatedAt = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            order.Customer = customer;

            return new OrderDto(order);
        }
    }
}