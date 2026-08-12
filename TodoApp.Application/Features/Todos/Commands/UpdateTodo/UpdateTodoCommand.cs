using FluentValidation;
using MediatR;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Application.Abstractions.Services;
using TodoApp.Application.DTOs;
using TodoApp.Domain.Enums;

namespace TodoApp.Application.Features.Todos.Commands.UpdateTodo;

/// <summary>
/// Var olan bir Todo'nun değiştirilebilir alanlarını güncelleme command'idir.
/// </summary>
// Bu command sadece degistirilebilir alanlari tasir; Id ve CreatedDate degistirilemez.
public sealed record UpdateTodoCommand(
    int Id,
    int UserId,
    string Title, 
    string? Description, 
    Priority Priority, 
    DateTime? DueDate,
    string? Assignee,
    string? Category,
    string? EstimatedTime,
    List<SubtaskDto>? Subtasks) : IRequest;

/// <summary>
/// Güncelleme isteğinin biçim kurallarını handler'dan ayırır.
/// </summary>
public sealed class UpdateTodoCommandValidator : AbstractValidator<UpdateTodoCommand>
{
    public UpdateTodoCommandValidator()
    {
        // URL'den gelen kimligin gecerli bir aday oldugunu kontrol eder.
        RuleFor(command => command.Id).GreaterThan(0);
        RuleFor(command => command.Title).NotEmpty().MaximumLength(200);
        RuleFor(command => command.Description).MaximumLength(2000);
        RuleFor(command => command.Priority).IsInEnum();
    }
}

/// <summary>
/// Kayıt bulunması, alanlarının değiştirilmesi ve kaydedilmesinden sorumludur.
/// </summary>
public sealed class UpdateTodoCommandHandler(ITodoRepository todoRepository, ITodoNotificationService notificationService) : IRequestHandler<UpdateTodoCommand>
{
    public async Task Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        // Kayit bulunamazsa sonraki satirlar calismaz ve API 404 cevabi verir.
        var todo = await todoRepository.GetByIdAsync(request.Id, request.UserId, cancellationToken)
            ?? throw new NotFoundException("Todo", request.Id);

        // Deger atama entity uzerinde yapilir; boylece kurallar tek yerde kalir.
        todo.UpdateDetails(request.Title, request.Description, request.Priority, request.DueDate, request.Assignee, request.Category, request.EstimatedTime);
        
        if (request.Subtasks != null)
        {
            todo.UpdateSubtasks(request.Subtasks.Select(s => (s.Title, s.Completed)));
        }
        else
        {
            todo.UpdateSubtasks(Enumerable.Empty<(string, bool)>());
        }

        await todoRepository.UpdateAsync(todo, cancellationToken);
        await todoRepository.SaveChangesAsync(cancellationToken);

        if (todo.RoomId.HasValue)
        {
            await notificationService.NotifyTaskUpdatedAsync(todo.RoomId.Value, cancellationToken);
        }
    }
}
