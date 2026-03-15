using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Common.Exceptions;
using Application.Orders.Dtos;
using Domain.Entities;
using Domain.Enums;

namespace Application.Orders.Commands.Confirm
{
    public sealed class ConfirmOrderHandler : ICommandHandler<ConfirmOrderCommand, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmOrderHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<OrderDto> Handle(ConfirmOrderCommand command, CancellationToken cancellationToken)
        {
            Order? order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException($"Order with id {command.OrderId} not found.");
            }

            if (order.Status != OrderStatus.Pending)
            {
                throw new ValidationException($"Only pending orders can be confirmed. Current status: {order.Status}");
            }

            if (order.OrderItems == null || order.OrderItems.Count == 0)
            {
                throw new ValidationException("Cannot confirm an order with no items.");
            }

            order.Status = OrderStatus.Confirmed;
            order.UpdatedAt = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new OrderDto(order);
        }
    }
}