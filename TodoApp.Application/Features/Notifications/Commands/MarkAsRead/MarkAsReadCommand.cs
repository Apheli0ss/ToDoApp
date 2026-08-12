using MediatR;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Features.Notifications.Commands.MarkAsRead;

public sealed record MarkAsReadCommand(int NotificationId, int UserId) : IRequest;

public sealed class MarkAsReadCommandHandler(INotificationRepository notificationRepository) : IRequestHandler<MarkAsReadCommand>
{
    public async Task Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    {
        var notification = await notificationRepository.GetByIdAsync(request.NotificationId, cancellationToken);
        if (notification == null || notification.UserId != request.UserId)
        {
            throw new NotFoundException(nameof(Notification), request.NotificationId);
        }

        notification.MarkAsRead();
        await notificationRepository.SaveChangesAsync(cancellationToken);
    }
}
