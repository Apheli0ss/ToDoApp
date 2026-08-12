using FluentValidation;
using MediatR;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Application.Abstractions.Services;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Domain.Enums;

namespace TodoApp.Application.Features.Todos.Commands.UpdateTodoStatus;

// Dosya adi eski bir ad tasir; siniflar tamamlanma bilgisi yerine tum durum degerini gunceller.
public sealed record UpdateTodoStatusCommand(int Id, int UserId, TodoStatus Status) : IRequest;

public sealed class UpdateTodoStatusCommandValidator : AbstractValidator<UpdateTodoStatusCommand>
{
    public UpdateTodoStatusCommandValidator()
    {
        // Gecersiz kimlikle veritabani sorgusu yapilmasini engeller.
        RuleFor(command => command.Id).GreaterThan(0);
        // Enum disinda bir sayinin durum olarak kaydedilmesini engeller.
        RuleFor(command => command.Status).IsInEnum();
    }
}

public sealed class UpdateTodoStatusCommandHandler(ITodoRepository todoRepository, ITodoNotificationService notificationService) : IRequestHandler<UpdateTodoStatusCommand>
{
    public async Task Handle(UpdateTodoStatusCommand request, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.GetByIdAsync(request.Id, request.UserId, cancellationToken)
            ?? throw new NotFoundException("Todo", request.Id);

        // Durum degisikligi entity'nin kendi metodu ile yapilir.
        todo.SetStatus(request.Status);
        await todoRepository.UpdateAsync(todo, cancellationToken);
        await todoRepository.SaveChangesAsync(cancellationToken);

        if (todo.RoomId.HasValue)
        {
            await notificationService.NotifyTaskUpdatedAsync(todo.RoomId.Value, cancellationToken);
        }
    }
}
