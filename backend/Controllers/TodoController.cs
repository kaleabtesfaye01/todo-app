using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly ILogger<TodoController> _logger;

        public TodoController(ITodoService todoService, ILogger<TodoController> logger)
        {
            _todoService = todoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            try
            {
                var todos = await _todoService.GetAllTodosAsync();
                return Ok(todos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting todos");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodo(int id)
        {
            try
            {
                var todo = await _todoService.GetTodoByIdAsync(id);
                if (todo == null)
                    return NotFound();

                return Ok(todo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting todo {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> CreateTodo(Todo todo)
        {
            try
            {
                var createdTodo = await _todoService.CreateTodoAsync(todo);
                return CreatedAtAction(nameof(GetTodo), new { id = createdTodo.Id }, createdTodo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating todo");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>> UpdateTodo(int id, Todo todo)
        {
            try
            {
                var updatedTodo = await _todoService.UpdateTodoAsync(id, todo);
                if (updatedTodo == null)
                    return NotFound();

                return Ok(updatedTodo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating todo {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            try
            {
                var result = await _todoService.DeleteTodoAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting todo {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPatch("{id}/toggle")]
        public async Task<ActionResult<Todo>> ToggleTodoStatus(int id)
        {
            try
            {
                var todo = await _todoService.ToggleTodoStatusAsync(id);
                if (todo == null)
                    return NotFound();

                return Ok(todo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling todo status {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
} 