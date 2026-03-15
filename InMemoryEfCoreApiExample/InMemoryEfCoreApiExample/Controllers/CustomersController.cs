using Application.Abstractions;
using Application.Customers.Commands.Create;
using Application.Customers.Commands.Delete;
using Application.Customers.Commands.Update;
using Application.Customers.Dtos;
using Application.Customers.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public CustomersController(IDispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<CustomerDto> customers = await _dispatcher.DispatchAsync(new GetAllCustomersQuery(), cancellationToken);
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetById(int id, CancellationToken cancellationToken)
        {
            CustomerDto customer = await _dispatcher.DispatchAsync(new GetCustomerByIdQuery(id), cancellationToken);
            return Ok(customer);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<CustomerDto>> GetByEmail(string email, CancellationToken cancellationToken)
        {
            CustomerDto customer = await _dispatcher.DispatchAsync(new GetCustomerByEmailQuery(email), cancellationToken);
            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            CustomerDto customer = await _dispatcher.DispatchAsync(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> Update(int id, [FromBody] UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest("ID mismatch");
            }

            CustomerDto customer = await _dispatcher.DispatchAsync(command, cancellationToken);
            return Ok(customer);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id, CancellationToken cancellationToken)
        {
            bool result = await _dispatcher.DispatchAsync(new DeleteCustomerCommand(id), cancellationToken);
            return Ok(result);
        }
    }
}