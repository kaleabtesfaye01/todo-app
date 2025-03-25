using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Data;
using backend.Services;
using TodoApi.DTOs;
using TodoApi.Interfaces;

namespace backend.Services;

public class TodoService : ITodoService
{
    private readonly TodoDbContext _context;

    public TodoService(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Todo>> GetAllTodosAsync()
    {
        return await _context.Todos.OrderByDescending(t => t.CreatedAt).ToListAsync();
    }

    public async Task<Todo?> GetTodoByIdAsync(int id)
    {
        return await _context.Todos.FindAsync(id);
    }

    public async Task<Todo> CreateTodoAsync(Todo todo)
    {
        todo.CreatedAt = DateTime.UtcNow;
        todo.UpdatedAt = DateTime.UtcNow;
        
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();
        
        return todo;
    }

    public async Task<Todo?> UpdateTodoAsync(int id, Todo todo)
    {
        var existingTodo = await _context.Todos.FindAsync(id);
        
        if (existingTodo == null)
            return null;

        existingTodo.Title = todo.Title;
        existingTodo.Description = todo.Description;
        existingTodo.Completed = todo.Completed;
        existingTodo.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        
        return existingTodo;
    }

    public async Task<bool> DeleteTodoAsync(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        
        if (todo == null)
            return false;

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<Todo?> ToggleTodoStatusAsync(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        
        if (todo == null)
            return null;

        todo.Completed = !todo.Completed;
        todo.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        
        return todo;
    }
} 