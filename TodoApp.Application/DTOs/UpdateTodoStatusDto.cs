using TodoApp.Domain.Enums;

namespace TodoApp.Application.DTOs;

/// <summary>Todo'nun yeni durumunu API'den almak için kullanılır.</summary>
public sealed class UpdateTodoStatusDto
{
    public TodoStatus Status { get; set; }
}
