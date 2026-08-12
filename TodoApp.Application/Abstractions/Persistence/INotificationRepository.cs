using TodoApp.Domain.Entities;

namespace TodoApp.Application.Abstractions.Persistence;

public interface INotificationRepository
{
    Task<IReadOnlyList<Notification>> GetUserNotificationsAsync(int userId, CancellationToken cancellationToken);
    Task<Notification?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Notification notification, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
