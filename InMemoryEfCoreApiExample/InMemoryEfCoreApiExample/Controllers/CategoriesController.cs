using Application.Abstractions;
using Application.Categories.Commands.Create;
using Application.Categories.Commands.Delete;
using Application.Categories.Commands.Update;
using Application.Categories.Dtos;
using Application.Categories.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public CategoriesController(IDispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<CategoryDto> categories = await _dispatcher.DispatchAsync(new GetAllCategoriesQuery(), cancellationToken);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id, CancellationToken cancellationToken)
        {
            CategoryDto category = await _dispatcher.DispatchAsync(new GetCategoryByIdQuery(id), cancellationToken);
            return Ok(category);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<CategoryDto>> GetByName(string name, CancellationToken cancellationToken)
        {
            CategoryDto category = await _dispatcher.DispatchAsync(new GetCategoryByName(name), cancellationToken);
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            CategoryDto category = await _dispatcher.DispatchAsync(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> Update(int id, [FromBody] UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest("ID mismatch");
            }

            CategoryDto category = await _dispatcher.DispatchAsync(command, cancellationToken);
            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id, CancellationToken cancellationToken)
        {
            bool result = await _dispatcher.DispatchAsync(new DeleteCategoryCommand(id), cancellationToken);
            return Ok(result);
        }
    }
}