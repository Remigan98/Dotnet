using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Common.Exceptions;
using Application.Orders.Dtos;
using Domain.Entities;

namespace Application.Orders.Commands.AddItem
{
    public sealed class AddOrderItemHandler : ICommandHandler<AddOrderItemCommand, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddOrderItemHandler(IOrderRepository orderRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._productRepository = productRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<OrderDto> Handle(AddOrderItemCommand command, CancellationToken cancellationToken)
        {
            Order? order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException($"Order with id {command.OrderId} not found.");
            }

            Product? product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException($"Product with id {command.ProductId} not found.");
            }

            // Check if product already exists in the order
            OrderItem? existingItem = order.OrderItems.FirstOrDefault(oi => oi.ProductId == command.ProductId);

            if (existingItem is not null)
            {
                throw new ValidationException($"Product '{product.Name}' already exists in this order. Use update instead.");
            }

            // Validate stock availability
            if (product.Stock < command.Quantity)
            {
                throw new ValidationException($"Insufficient stock for product '{product.Name}'. Available: {product.Stock}, Requested: {command.Quantity}");
            }

            // Create new order item
            OrderItem orderItem = new OrderItem
            {
                OrderId = command.OrderId,
                ProductId = command.ProductId,
                Quantity = command.Quantity,
                UnitPrice = product.Price,
                Product = product
            };

            order.OrderItems.Add(orderItem);

            // Update order total amount
            order.TotalAmount += orderItem.Quantity * orderItem.UnitPrice;

            // Update product stock
            product.Stock -= command.Quantity;
            await _productRepository.UpdateAsync(product, cancellationToken);

            // Update order
            await _orderRepository.UpdateAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new OrderDto(order);
        }
    }
}