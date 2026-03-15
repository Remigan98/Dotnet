using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Common.Exceptions;
using Application.Orders.Dtos;
using Domain.Entities;

namespace Application.Orders.Commands.UpdateItemQuantity
{
    public sealed class UpdateOrderItemQuantityHandler : ICommandHandler<UpdateOrderItemQuantityCommand, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderItemQuantityHandler(IOrderRepository orderRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._productRepository = productRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<OrderDto> Handle(UpdateOrderItemQuantityCommand command, CancellationToken cancellationToken)
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

            // Calculate the quantity difference
            int quantityDifference = command.NewQuantity - orderItem.Quantity;

            // Validate stock availability if increasing quantity
            if (quantityDifference > 0 && product.Stock < quantityDifference)
            {
                throw new ValidationException($"Insufficient stock for product '{product.Name}'. Available: {product.Stock}, Required: {quantityDifference}");
            }

            // Update product stock
            product.Stock -= quantityDifference;
            await _productRepository.UpdateAsync(product, cancellationToken);

            // Update order total amount (subtract old amount, add new amount)
            order.TotalAmount -= orderItem.Quantity * orderItem.UnitPrice;
            order.TotalAmount += command.NewQuantity * orderItem.UnitPrice;

            // Update order item quantity
            orderItem.Quantity = command.NewQuantity;

            // Update order
            order.UpdatedAt = DateTime.UtcNow;
            await _orderRepository.UpdateAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new OrderDto(order);
        }
    }
}