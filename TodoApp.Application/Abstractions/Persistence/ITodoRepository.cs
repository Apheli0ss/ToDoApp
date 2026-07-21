using TodoApp.Domain.Entities;

namespace TodoApp.Application.Abstractions.Persistence;

/// <summary>
/// Todo verisine erişmek için Application katmanının ihtiyaç duyduğu sözleşme.
/// Implementasyon Persistence katmanında bulunur; Application EF Core'u bilmez.
/// </summary>
public interface ITodoRepository
{
    Task<List<TodoItem>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TodoItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task AddAsync(TodoItem todo, CancellationToken cancellationToken = default);

    Task UpdateAsync(TodoItem todo, CancellationToken cancellationToken = default);

    Task DeleteAsync(TodoItem todo, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
