using TodoApp.Application.DTOs;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Features.Todos;

/// <summary>
/// Todo feature'ının entity-to-DTO dönüşümlerini tek noktada tutar.
/// </summary>
public static class TodoMappings
{
    // "this" anahtar kelimesi, todo.ToDto() kisaltmasini kullanilabilir hale getirir.
    // Entity'nin tum okunabilir alanlari yeni DTO'ya tek tek kopyalanir.
    public static TodoDto ToDto(this TodoItem todo) => new()
    {
        Id = todo.Id,
        Title = todo.Title,
        Description = todo.Description,
        Status = todo.Status,
        Priority = todo.Priority,
        CreatedDate = todo.CreatedDate,
        DueDate = todo.DueDate,
        Assignee = todo.Assignee,
        Category = todo.Category,
        EstimatedTime = todo.EstimatedTime,
        Subtasks = todo.Subtasks.Select(s => new SubtaskDto 
        { 
            Id = s.Id, 
            Title = s.Title, 
            Completed = s.Completed 
        }).ToList()
    };

    // Koleksiyon donusumunde Select her entity icin yukaridaki tekil donusumu calistirir.
    public static IReadOnlyList<TodoDto> ToDto(this IEnumerable<TodoItem> todos) =>
        todos.Select(ToDto).ToList();
}
