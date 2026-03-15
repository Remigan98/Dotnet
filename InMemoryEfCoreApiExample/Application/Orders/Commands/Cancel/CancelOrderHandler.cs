using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Common.Exceptions;
using Application.Orders.Dtos;
using Domain.Entities;
using Domain.Enums;

namespace Application.Orders.Commands.Cancel
{
    public sealed class CancelOrderHandler : ICommandHandler<CancelOrderCommand, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelOrderHandler(IOrderRepository orderRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._productRepository = productRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<OrderDto> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
        {
            Order? order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException($"Order with id {command.OrderId} not found.");
            }

            if (order.Status == OrderStatus.Cancelled)
            {
                throw new ValidationException("Order is already cancelled.");
            }

            if (order.Status == OrderStatus.Delivered)
            {
                throw new ValidationException("Cannot cancel a delivered order.");
            }

            // Restore product stock for all order items
            foreach (OrderItem orderItem in order.OrderItems)
            {
                Product? product = await _productRepository.GetByIdAsync(orderItem.ProductId, cancellationToken);

                if (product is not null)
                {
                    product.Stock += orderItem.Quantity;
                    await _productRepository.UpdateAsync(product, cancellationToken);
                }
            }

            order.Status = OrderStatus.Cancelled;
            order.UpdatedAt = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new OrderDto(order);
        }
    }
}