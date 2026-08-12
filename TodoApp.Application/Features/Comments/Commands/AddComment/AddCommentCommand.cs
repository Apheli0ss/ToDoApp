using MediatR;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Domain.Entities;
using TodoApp.Application.Abstractions.Services;

namespace TodoApp.Application.Features.Comments.Commands.AddComment;

public sealed record AddCommentCommand(
    int TodoItemId,
    int UserId,
    string Content
) : IRequest<int>;

public sealed class AddCommentCommandHandler(
    ICommentRepository commentRepository, 
    ITodoRepository todoRepository,
    ITodoNotificationService notificationService,
    INotificationRepository notifRepository) : IRequestHandler<AddCommentCommand, int>
{
    public async Task<int> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.GetByIdAsync(request.TodoItemId, request.UserId, cancellationToken);
        if (todo == null)
        {
            throw new Exception("Todo item not found");
        }

        var comment = new Comment
        {
            TodoItemId = request.TodoItemId,
            UserId = request.UserId,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        await commentRepository.AddAsync(comment, cancellationToken);
        await commentRepository.SaveChangesAsync(cancellationToken);

        // Notify room members via SignalR
        if (todo.RoomId.HasValue)
        {
            await notificationService.NotifyTaskUpdatedAsync(todo.RoomId.Value, cancellationToken);
        }

        // Create persistent notification for the task owner if someone else comments
        if (todo.UserId != request.UserId)
        {
            var notif = new Notification(todo.UserId, $"Görevinize ({todo.Title}) yeni bir yorum eklendi.");
            await notifRepository.AddAsync(notif, cancellationToken);
            await notifRepository.SaveChangesAsync(cancellationToken);
        }

        return comment.Id;
    }
}
