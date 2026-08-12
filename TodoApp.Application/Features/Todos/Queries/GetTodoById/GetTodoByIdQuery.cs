using MediatR;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.DTOs;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Application.Features.Todos;

namespace TodoApp.Application.Features.Todos.Queries.GetTodoById;

/// <summary>
/// Kimliği verilen Todo kaydını okumak için kullanılan CQRS query'sidir.
/// </summary>
public sealed record GetTodoByIdQuery(int Id, int UserId) : IRequest<TodoDto>;

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
        // null sonucunu istemciye belirsiz bicimde gondermek yerine kayit yok hatasi uretilir.
        var todo = await todoRepository.GetByIdAsync(request.Id, request.UserId, cancellationToken)
            ?? throw new NotFoundException("Todo", request.Id);

        // Entity, API'nin disariya acacagi DTO nesnesine donusturulur.
        return todo.ToDto();
    }
}
