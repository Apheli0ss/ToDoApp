using Microsoft.AspNetCore.SignalR;
using TodoApp.Application.Abstractions.Services;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.API.Hubs;

namespace TodoApp.API.Services;

public class TodoNotificationService : ITodoNotificationService
{
    private readonly IHubContext<TodoHub> _hubContext;
    private readonly IRoomRepository _roomRepository;
    private readonly INotificationRepository _notificationRepository;

    public TodoNotificationService(IHubContext<TodoHub> hubContext, IRoomRepository roomRepository, INotificationRepository notificationRepository)
    {
        _hubContext = hubContext;
        _roomRepository = roomRepository;
        _notificationRepository = notificationRepository;
    }

    public async Task NotifyTaskUpdatedAsync(int roomId, CancellationToken cancellationToken = default)
    {
        await NotifyTaskUpdatedAsync(roomId, "Odanızda bir görev güncellendi veya yeni bir yorum yapıldı.", cancellationToken);
    }

    public async Task NotifyTaskUpdatedAsync(int roomId, string message, CancellationToken cancellationToken = default)
    {
        var room = await _roomRepository.GetByIdAsync(roomId, cancellationToken);
        if (room != null)
        {
            // "TaskUpdated" eventi fırlatılır.
            await _hubContext.Clients.Group(room.RoomCode).SendCoreAsync("TaskUpdated", Array.Empty<object>(), cancellationToken);

            // Create DB Notifications for all room members
            foreach (var member in room.RoomMembers)
            {
                var notification = new TodoApp.Domain.Entities.Notification(member.UserId, message);
                await _notificationRepository.AddAsync(notification, cancellationToken);
            }
            await _notificationRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
