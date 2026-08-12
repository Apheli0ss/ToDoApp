using MediatR;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Features.Notifications.Queries.GetNotifications;

public record NotificationDto(int Id, string Message, bool IsRead, DateTime CreatedAt);

public sealed record GetNotificationsQuery(int UserId) : IRequest<IReadOnlyList<NotificationDto>>;

public sealed class GetNotificationsQueryHandler(INotificationRepository notificationRepository) : IRequestHandler<GetNotificationsQuery, IReadOnlyList<NotificationDto>>
{
    public async Task<IReadOnlyList<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        var notifications = await notificationRepository.GetUserNotificationsAsync(request.UserId, cancellationToken);
        return notifications.Select(n => new NotificationDto(n.Id, n.Message, n.IsRead, n.CreatedAt)).ToList();
    }
}
