using Application.Abstractions;
using Application.Products.Commands.Create;
using Application.Products.Commands.Delete;
using Application.Products.Commands.Update;
using Application.Products.Dtos;
using Application.Products.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public ProductsController(IDispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<ProductDto> products = await _dispatcher.DispatchAsync(new GetAllProductsQuery(), cancellationToken);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id, CancellationToken cancellationToken)
        {
            ProductDto product = await _dispatcher.DispatchAsync(new GetProductByIdQuery(id), cancellationToken);
            return Ok(product);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetByCategoryId(int categoryId, CancellationToken cancellationToken)
        {
            IEnumerable<ProductDto> products = await _dispatcher.DispatchAsync(new GetProductsByCategoryIdQuery(categoryId), cancellationToken);
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
        {
            ProductDto product = await _dispatcher.DispatchAsync(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> Update(int id, [FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest("ID mismatch");
            }

            ProductDto product = await _dispatcher.DispatchAsync(command, cancellationToken);
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id, CancellationToken cancellationToken)
        {
            bool result = await _dispatcher.DispatchAsync(new DeleteProductCommand(id), cancellationToken);
            return Ok(result);
        }
    }
}