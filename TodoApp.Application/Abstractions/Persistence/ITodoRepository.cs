using TodoApp.Domain.Entities;

namespace TodoApp.Application.Abstractions.Persistence;

/// <summary>
/// Todo verisine erişmek için Application katmanının ihtiyaç duyduğu sözleşme.
/// Implementasyon Persistence katmanında bulunur; Application EF Core'u bilmez.
/// </summary>
public interface ITodoRepository
{
    Task<(List<TodoItem> Items, int TotalCount)> GetAllAsync(int userId, int? roomId, int skip, int take, CancellationToken cancellationToken);

    Task<TodoItem?> GetByIdAsync(int id, int userId, CancellationToken cancellationToken);

    Task AddAsync(TodoItem todo, CancellationToken cancellationToken);

    Task UpdateAsync(TodoItem todo, CancellationToken cancellationToken);

    Task DeleteAsync(TodoItem todo, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
