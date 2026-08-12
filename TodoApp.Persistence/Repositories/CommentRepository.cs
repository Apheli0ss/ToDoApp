using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Domain.Entities;
using TodoApp.Persistence;

namespace TodoApp.Persistence.Repositories;

public class CommentRepository(AppDbContext context) : ICommentRepository
{
    public async Task<IReadOnlyList<Comment>> GetByTodoItemIdAsync(int todoItemId, CancellationToken cancellationToken)
    {
        return await context.Comments
            .Include(c => c.User)
            .Where(c => c.TodoItemId == todoItemId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Comment comment, CancellationToken cancellationToken)
    {
        await context.Comments.AddAsync(comment, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
