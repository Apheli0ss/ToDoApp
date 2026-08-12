using FluentValidation;
using MediatR;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Application.Abstractions.Services;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.DTOs;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Enums;

namespace TodoApp.Application.Features.Todos.Commands.CreateTodo;

// record, bu istegin tasidigi alanlari kisa ve guvenli bicimde bir araya getirir.
// IRequest<int>, basarili sonucta yeni kaydin Id degerinin dondugunu belirtir.
public sealed record CreateTodoCommand(
    int UserId,
    string Title,
    string? Description,
    Priority Priority,
    DateTime? DueDate,
    string? Assignee,
    string? Category,
    string? EstimatedTime,
    List<SubtaskDto>? Subtasks,
    int? RoomId = null) : IRequest<int>;

public sealed class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoCommandValidator()
    {
        // Baslik zorunludur ve veritabani/arayuz icin makul uzunlukta tutulur.
        RuleFor(command => command.Title).NotEmpty().MaximumLength(200);
        // Aciklama bos olabilir; ancak verilirse siniri asmamalidir.
        RuleFor(command => command.Description).MaximumLength(2000);
        // Gecersiz sayisal bir enum degerinin kaydedilmesini engeller.
        RuleFor(command => command.Priority).IsInEnum();
        RuleFor(command => command.DueDate)
            .Must(date => !date.HasValue || date.Value.Date >= DateTime.Today)
            .WithMessage("Son teslim tarihi bugünden önce olamaz.");
    }
}

public sealed class CreateTodoCommandHandler(
    ITodoRepository todoRepository,
    IRoomRepository roomRepository,
    ITodoNotificationService notificationService)
    : IRequestHandler<CreateTodoCommand, int>
{
    public async Task<int> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        if (request.RoomId.HasValue)
        {
            var room = await roomRepository.GetByIdAsync(request.RoomId.Value, cancellationToken);
            if (room is null)
                throw new NotFoundException("Ortak oda bulunamadı.");

            if (!room.RoomMembers.Any(member => member.UserId == request.UserId))
                throw new UnauthorizedAccessException("Bu ortak odada görev oluşturma yetkiniz yok.");
        }

        var todo = new TodoItem(request.UserId, request.Title, request.Description, request.Priority, request.DueDate, request.Assignee, request.Category, request.EstimatedTime, request.RoomId);
        
        if (request.Subtasks != null && request.Subtasks.Any())
        {
            todo.UpdateSubtasks(request.Subtasks.Select(s => (s.Title, s.Completed)));
        }

        await todoRepository.AddAsync(todo, cancellationToken);
        // Bu noktada bekleyen ekleme islemi veritabanina yazilir.
        await todoRepository.SaveChangesAsync(cancellationToken);

        if (request.RoomId.HasValue)
        {
            await notificationService.NotifyTaskUpdatedAsync(request.RoomId.Value, cancellationToken);
        }

        return todo.Id;
    }
}
