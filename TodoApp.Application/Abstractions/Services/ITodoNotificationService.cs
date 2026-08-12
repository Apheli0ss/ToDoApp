namespace TodoApp.Application.Abstractions.Services;

public interface ITodoNotificationService
{
    /// <summary>
    /// Belli bir odaya bağlı istemcilere görevin güncellendiği bilgisini iletir.
    /// </summary>
    Task NotifyTaskUpdatedAsync(int roomId, CancellationToken cancellationToken = default);
    Task NotifyTaskUpdatedAsync(int roomId, string message, CancellationToken cancellationToken = default);
}
