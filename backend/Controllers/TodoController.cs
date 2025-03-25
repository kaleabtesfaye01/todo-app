using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Interfaces;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodosController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetTodos()
    {
        var todos = await _todoService.GetAllTodosAsync();
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoDto>> GetTodo(int id)
    {
        var todo = await _todoService.GetTodoByIdAsync(id);
        if (todo == null)
            return NotFound();

        return Ok(todo);
    }

    [HttpPost]
    public async Task<ActionResult<TodoDto>> CreateTodo(CreateTodoDto createTodoDto)
    {
        var todo = await _todoService.CreateTodoAsync(createTodoDto);
        return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TodoDto>> UpdateTodo(int id, UpdateTodoDto updateTodoDto)
    {
        var todo = await _todoService.UpdateTodoAsync(id, updateTodoDto);
        if (todo == null)
            return NotFound();

        return Ok(todo);
    }

    [HttpPatch("{id}/toggle")]
    public async Task<ActionResult<TodoDto>> ToggleTodoStatus(int id, [FromBody] bool completed)
    {
        var todo = await _todoService.ToggleTodoStatusAsync(id, completed);
        if (todo == null)
            return NotFound();

        return Ok(todo);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var result = await _todoService.DeleteTodoAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
} 