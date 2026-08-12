using TodoApp.Domain.Entities;

namespace TodoApp.Application.Abstractions.Persistence;

public interface ICommentRepository
{
    Task<IReadOnlyList<Comment>> GetByTodoItemIdAsync(int todoItemId, CancellationToken cancellationToken);
    Task AddAsync(Comment comment, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
