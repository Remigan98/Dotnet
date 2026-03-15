using Application.Abstractions;
using Application.Orders.Commands.AddItem;
using Application.Orders.Commands.Cancel;
using Application.Orders.Commands.Confirm;
using Application.Orders.Commands.Create;
using Application.Orders.Commands.Delete;
using Application.Orders.Commands.RemoveItem;
using Application.Orders.Commands.Update;
using Application.Orders.Commands.UpdateItemQuantity;
using Application.Orders.Dtos;
using Application.Orders.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public OrdersController(IDispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<OrderDto> orders = await _dispatcher.DispatchAsync(new GetAllOrdersQuery(), cancellationToken);

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(int id, CancellationToken cancellationToken)
        {
            OrderDto order = await _dispatcher.DispatchAsync(new GetOrderByIdQuery(id), cancellationToken);

            return Ok(order);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetByCustomerId(int customerId, CancellationToken cancellationToken)
        {
            IEnumerable<OrderDto> orders = await _dispatcher.DispatchAsync(new GetOrdersByCustomerIdQuery(customerId), cancellationToken);

            return Ok(orders);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetByStatus(OrderStatus status, CancellationToken cancellationToken)
        {
            IEnumerable<OrderDto> orders = await _dispatcher.DispatchAsync(new GetOrdersByStatusQuery(status), cancellationToken);

            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderCommand command, CancellationToken cancellationToken)
        {
            OrderDto order = await _dispatcher.DispatchAsync(command, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDto>> Update(int id, [FromBody] UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest("ID mismatch");
            }

            OrderDto order = await _dispatcher.DispatchAsync(command, cancellationToken);


            return Ok(order);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id, CancellationToken cancellationToken)
        {
            bool result = await _dispatcher.DispatchAsync(new DeleteOrderCommand(id), cancellationToken);

            return Ok(result);
        }

        [HttpPost("{orderId}/items")]
        public async Task<ActionResult<OrderDto>> AddItem(int orderId, [FromBody] AddOrderItemRequest request, CancellationToken cancellationToken)
        {
            var command = new AddOrderItemCommand(orderId, request.ProductId, request.Quantity);
            OrderDto order = await _dispatcher.DispatchAsync(command, cancellationToken);

            return Ok(order);
        }

        [HttpDelete("{orderId}/items/{productId}")]
        public async Task<ActionResult<OrderDto>> RemoveItem(int orderId, int productId, CancellationToken cancellationToken)
        {
            RemoveOrderItemCommand command = new RemoveOrderItemCommand(orderId, productId);
            OrderDto order = await _dispatcher.DispatchAsync(command, cancellationToken);

            return Ok(order);
        }

        [HttpPut("{orderId}/items/{productId}/quantity")]
        public async Task<ActionResult<OrderDto>> UpdateItemQuantity(int orderId, int productId, [FromBody] UpdateQuantityRequest request, CancellationToken cancellationToken)
        {
            UpdateOrderItemQuantityCommand command = new UpdateOrderItemQuantityCommand(orderId, productId, request.NewQuantity);
            OrderDto order = await _dispatcher.DispatchAsync(command, cancellationToken);

            return Ok(order);
        }

        [HttpPost("{orderId}/confirm")]
        public async Task<ActionResult<OrderDto>> Confirm(int orderId, CancellationToken cancellationToken)
        {
            ConfirmOrderCommand command = new ConfirmOrderCommand(orderId);
            OrderDto order = await _dispatcher.DispatchAsync(command, cancellationToken);

            return Ok(order);
        }

        [HttpPost("{orderId}/cancel")]
        public async Task<ActionResult<OrderDto>> Cancel(int orderId, CancellationToken cancellationToken)
        {
            CancelOrderCommand command = new CancelOrderCommand(orderId);
            OrderDto order = await _dispatcher.DispatchAsync(command, cancellationToken);

            return Ok(order);
        }
    }

    public record AddOrderItemRequest(int ProductId, int Quantity);
    public record UpdateQuantityRequest(int NewQuantity);
}