using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Services;

public class TodoService : ITodoService
{
    private readonly TodoDbContext _context;

    public TodoService(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoDto>> GetAllTodosAsync()
    {
        return await _context.Todos
            .Select(t => new TodoDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Completed = t.Completed,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<TodoDto?> GetTodoByIdAsync(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null) return null;

        return new TodoDto
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            Completed = todo.Completed,
            CreatedAt = todo.CreatedAt,
            UpdatedAt = todo.UpdatedAt
        };
    }

    public async Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto)
    {
        var todo = new Todo
        {
            Title = createTodoDto.Title,
            Description = createTodoDto.Description,
            Completed = createTodoDto.Completed,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        return new TodoDto
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            Completed = todo.Completed,
            CreatedAt = todo.CreatedAt,
            UpdatedAt = todo.UpdatedAt
        };
    }

    public async Task<TodoDto?> UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null) return null;

        if (updateTodoDto.Title != null)
            todo.Title = updateTodoDto.Title;
        if (updateTodoDto.Description != null)
            todo.Description = updateTodoDto.Description;
        if (updateTodoDto.Completed.HasValue)
            todo.Completed = updateTodoDto.Completed.Value;

        todo.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new TodoDto
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            Completed = todo.Completed,
            CreatedAt = todo.CreatedAt,
            UpdatedAt = todo.UpdatedAt
        };
    }

    public async Task<TodoDto?> ToggleTodoStatusAsync(int id, bool completed)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null) return null;

        todo.Completed = completed;
        todo.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new TodoDto
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            Completed = todo.Completed,
            CreatedAt = todo.CreatedAt,
            UpdatedAt = todo.UpdatedAt
        };
    }

    public async Task<bool> DeleteTodoAsync(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null) return false;

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();

        return true;
    }
} 