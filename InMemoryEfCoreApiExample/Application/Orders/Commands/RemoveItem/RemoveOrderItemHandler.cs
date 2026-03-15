using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Common.Exceptions;
using Application.Orders.Dtos;
using Domain.Entities;

namespace Application.Orders.Commands.RemoveItem
{
    public sealed class RemoveOrderItemHandler : ICommandHandler<RemoveOrderItemCommand, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveOrderItemHandler(IOrderRepository orderRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._productRepository = productRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<OrderDto> Handle(RemoveOrderItemCommand command, CancellationToken cancellationToken)
        {
            Order? order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException($"Order with id {command.OrderId} not found.");
            }

            OrderItem? orderItem = order.OrderItems.FirstOrDefault(oi => oi.ProductId == command.ProductId);

            if (orderItem is null)
            {
                throw new NotFoundException($"Product with id {command.ProductId} not found in this order.");
            }

            Product? product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException($"Product with id {command.ProductId} not found.");
            }

            // Restore product stock
            product.Stock += orderItem.Quantity;
            await _productRepository.UpdateAsync(product, cancellationToken);

            // Update order total amount
            order.TotalAmount -= orderItem.Quantity * orderItem.UnitPrice;

            // Remove order item
            order.OrderItems.Remove(orderItem);

            // Update order
            await _orderRepository.UpdateAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new OrderDto(order);
        }
    }
}