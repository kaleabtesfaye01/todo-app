using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options)
    {
    }

    public DbSet<Todo> Todos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todo>()
            .Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);

        modelBuilder.Entity<Todo>()
            .Property(t => t.Description)
            .HasMaxLength(1000);

        modelBuilder.Entity<Todo>()
            .Property(t => t.CreatedAt)
            .HasDefaultValueSql("DATETIME('now')");

        modelBuilder.Entity<Todo>()
            .Property(t => t.UpdatedAt)
            .HasDefaultValueSql("DATETIME('now')");

        // Add some seed data
        modelBuilder.Entity<Todo>().HasData(
            new Todo
            {
                Id = 1,
                Title = "Welcome to Todo App",
                Description = "This is your first todo item. You can edit or delete it.",
                Completed = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
    }
} 