using MediatR;
using TodoApp.Application.DTOs;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Application.Features.Todos;

using TodoApp.Application.Common.Models;

namespace TodoApp.Application.Features.Todos.Queries.GetTodos;

/// <summary>
/// Tüm Todo kayıtlarını okumak için kullanılan, yan etkisiz CQRS query'sidir.
/// </summary>
public sealed record GetTodosQuery(int UserId, int? RoomId, int PageNumber = 1, int PageSize = 50) : IRequest<PagedResult<TodoDto>>;

/// <summary>
/// GetTodosQuery'nin yalnızca okuma sorumluluğunu taşıyan handler'ıdır.
/// </summary>
public sealed class GetTodosQueryHandler(ITodoRepository todoRepository)
    : IRequestHandler<GetTodosQuery, PagedResult<TodoDto>>
{
    /// <summary>
    /// Repository'den entity'leri alır ve API sözleşmesi olan DTO'lara çevirir.
    /// </summary>
    public async Task<PagedResult<TodoDto>> Handle(
        GetTodosQuery request,
        CancellationToken cancellationToken)
    {
        var skip = (request.PageNumber - 1) * request.PageSize;
        
        var result = await todoRepository.GetAllAsync(request.UserId, request.RoomId, skip, request.PageSize, cancellationToken);

        var dtos = result.Items.ToDto();
        var hasNextPage = (skip + request.PageSize) < result.TotalCount;
        
        return new PagedResult<TodoDto>(dtos.ToList(), result.TotalCount, hasNextPage);
    }
}
