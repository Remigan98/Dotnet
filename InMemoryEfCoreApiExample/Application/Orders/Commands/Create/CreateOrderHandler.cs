using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Common.Exceptions;
using Application.Orders.Dtos;
using Domain.Entities;

namespace Application.Orders.Commands.Create
{
    public sealed class CreateOrderHandler : ICommandHandler<CreateOrderCommand, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderHandler(
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._customerRepository = customerRepository;
            this._productRepository = productRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<OrderDto> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            Customer? customer = await _customerRepository.GetByIdAsync(command.CustomerId, cancellationToken);

            if (customer is null)
            {
                throw new NotFoundException($"Customer with id {command.CustomerId} not found.");
            }

            Order order = new Order
            {
                CustomerId = command.CustomerId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = 0
            };

            decimal totalAmount = 0;

            foreach (CreateOrderItemCommand item in command.OrderItems)
            {
                Product? product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);

                if (product is null)
                {
                    throw new NotFoundException($"Product with id {item.ProductId} not found.");
                }

                // Validate stock availability
                if (product.Stock < item.Quantity)
                {
                    throw new ValidationException($"Insufficient stock for product '{product.Name}'. Available: {product.Stock}, Requested: {item.Quantity}");
                }

                // Create order item
                OrderItem orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                };

                order.OrderItems.Add(orderItem);
                totalAmount += orderItem.Quantity * orderItem.UnitPrice;

                // Update product stock
                product.Stock -= item.Quantity;
                await _productRepository.UpdateAsync(product, cancellationToken);
            }

            order.TotalAmount = totalAmount;

            // Save order
            await _orderRepository.AddAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Load the order with related entities for the DTO
            order.Customer = customer;

            foreach (OrderItem orderItem in order.OrderItems)
            {
                Product? product = await _productRepository.GetByIdAsync(orderItem.ProductId, cancellationToken);

                if (product is not null)
                {
                    orderItem.Product = product;
                }
            }

            return new OrderDto(order);
        }
    }
}