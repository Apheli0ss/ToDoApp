using TodoApp.Application.DTOs;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Features.Todos;

/// <summary>
/// Todo feature'ının entity-to-DTO dönüşümlerini tek noktada tutar.
/// </summary>
public static class TodoMappings
{
    public static TodoDto ToDto(this TodoItem todo) => new()
    {
        Id = todo.Id,
        Title = todo.Title,
        Description = todo.Description,
        Status = todo.Status,
        Priority = todo.Priority,
        CreatedDate = todo.CreatedDate,
        DueDate = todo.DueDate
    };

    public static IReadOnlyList<TodoDto> ToDto(this IEnumerable<TodoItem> todos) =>
        todos.Select(ToDto).ToList();
}
