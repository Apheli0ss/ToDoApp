using MediatR;
using TodoApp.Application.DTOs;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Application.Features.Todos;

namespace TodoApp.Application.Features.Todos.Queries.GetTodos;

/// <summary>
/// Tüm Todo kayıtlarını okumak için kullanılan, yan etkisiz CQRS query'sidir.
/// </summary>
public sealed record GetTodosQuery : IRequest<IReadOnlyList<TodoDto>>;

/// <summary>
/// GetTodosQuery'nin yalnızca okuma sorumluluğunu taşıyan handler'ıdır.
/// </summary>
public sealed class GetTodosQueryHandler(ITodoRepository todoRepository)
    : IRequestHandler<GetTodosQuery, IReadOnlyList<TodoDto>>
{
    /// <summary>
    /// Repository'den entity'leri alır ve API sözleşmesi olan DTO'lara çevirir.
    /// </summary>
    public async Task<IReadOnlyList<TodoDto>> Handle(
        GetTodosQuery request,
        CancellationToken cancellationToken)
    {
        var todos = await todoRepository.GetAllAsync(cancellationToken);

        return todos.ToDto();
    }
}
