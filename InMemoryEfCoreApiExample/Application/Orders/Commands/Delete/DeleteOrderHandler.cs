using Application.Abstractions;
using Application.Abstractions.Persistence;
using Application.Common.Exceptions;
using Domain.Entities;

namespace Application.Orders.Commands.Delete
{
    public sealed class DeleteOrderHandler : ICommandHandler<DeleteOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOrderHandler(IOrderRepository orderRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._productRepository = productRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            Order? order = await _orderRepository.GetByIdAsync(command.Id, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException($"Order with id {command.Id} not found.");
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

            _orderRepository.Delete(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            order = await _orderRepository.GetByIdAsync(command.Id, cancellationToken);

            return order is null;
        }
    }
}