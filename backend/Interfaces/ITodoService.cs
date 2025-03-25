using TodoApi.DTOs;

namespace TodoApi.Interfaces;

public interface ITodoService
{
    Task<IEnumerable<TodoDto>> GetAllTodosAsync();
    Task<TodoDto?> GetTodoByIdAsync(int id);
    Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto);
    Task<TodoDto?> UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto);
    Task<TodoDto?> ToggleTodoStatusAsync(int id, bool completed);
    Task<bool> DeleteTodoAsync(int id);
} 