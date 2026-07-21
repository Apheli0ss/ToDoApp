using MediatR;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.DTOs;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Application.Features.Todos;

namespace TodoApp.Application.Features.Todos.Queries.GetTodoById;

/// <summary>
/// Kimliği verilen Todo kaydını okumak için kullanılan CQRS query'sidir.
/// </summary>
public sealed record GetTodoByIdQuery(int Id) : IRequest<TodoDto>;

/// <summary>
/// Tek kaydın bulunması ve DTO'ya dönüştürülmesinden sorumludur.
/// </summary>
public sealed class GetTodoByIdQueryHandler(ITodoRepository todoRepository)
    : IRequestHandler<GetTodoByIdQuery, TodoDto>
{
    /// <summary>
    /// Kayıt yoksa HTTP ayrıntısı içermeyen application exception'ı üretir.
    /// </summary>
    public async Task<TodoDto> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(TodoDto), request.Id);

        return todo.ToDto();
    }
}
