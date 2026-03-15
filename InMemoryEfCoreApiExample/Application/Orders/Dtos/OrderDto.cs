using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Orders.Dtos
{
    public sealed record OrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = default!;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

        public OrderDto() { }

        public OrderDto(int id, int customerId, string customerName, DateTime orderDate, decimal totalAmount, OrderStatus status, List<OrderItemDto> orderItems)
        {
            Id = id;
            CustomerId = customerId;
            CustomerName = customerName;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
            Status = status;
            OrderItems = orderItems;
        }

        public OrderDto(Order order)
        {
            ArgumentNullException.ThrowIfNull(order);

            Id = order.Id;
            CustomerId = order.CustomerId;
            CustomerName = order.Customer != null 
                ? $"{order.Customer.FirstName} {order.Customer.LastName}" 
                : string.Empty;
            OrderDate = order.OrderDate;
            TotalAmount = order.TotalAmount;
            Status = order.Status;
            OrderItems = order.OrderItems?.Select(oi => new OrderItemDto(oi)).ToList() ?? new List<OrderItemDto>();
        }

        public OrderDto(OrderDto copy)
        {
            ArgumentNullException.ThrowIfNull(copy);

            Id = copy.Id;
            CustomerId = copy.CustomerId;
            CustomerName = copy.CustomerName;
            OrderDate = copy.OrderDate;
            TotalAmount = copy.TotalAmount;
            Status = copy.Status;
            OrderItems = copy.OrderItems?.Select(oi => new OrderItemDto(oi)).ToList() ?? new List<OrderItemDto>();
        }
    }
}
