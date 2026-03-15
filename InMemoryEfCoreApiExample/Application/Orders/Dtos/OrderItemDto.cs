using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Dtos
{
    public sealed record OrderItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal => Quantity * UnitPrice;

        public OrderItemDto() { }

        public OrderItemDto(int id, int productId, string productName, int quantity, decimal unitPrice)
        {
            Id = id;
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public OrderItemDto(OrderItem orderItem)
        {
            ArgumentNullException.ThrowIfNull(orderItem);

            Id = orderItem.Id;
            ProductId = orderItem.ProductId;
            ProductName = orderItem.Product?.Name ?? string.Empty;
            Quantity = orderItem.Quantity;
            UnitPrice = orderItem.UnitPrice;
        }

        public OrderItemDto(OrderItemDto copy)
        {
            ArgumentNullException.ThrowIfNull(copy);

            Id = copy.Id;
            ProductId = copy.ProductId;
            ProductName = copy.ProductName;
            Quantity = copy.Quantity;
            UnitPrice = copy.UnitPrice;
        }
    }
}