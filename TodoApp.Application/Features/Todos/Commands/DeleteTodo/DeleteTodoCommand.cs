using FluentValidation;
using MediatR;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Application.Abstractions.Services;

namespace TodoApp.Application.Features.Todos.Commands.DeleteTodo;

/// <summary>
/// Bir Todo kaydını silmek için kullanılan durum değiştiren command'dir.
/// </summary>
// IRequest sonuc verisi tasimaz; basarili silmede yalnizca HTTP durum kodu yeterlidir.
public sealed record DeleteTodoCommand(int Id, int UserId) : IRequest;

/// <summary>
/// Silme command'inin kimlik kuralını tanımlar.
/// </summary>
public sealed class DeleteTodoCommandValidator : AbstractValidator<DeleteTodoCommand>
{
    // Sifir ve negatif degerler gecerli bir veritabani kimligi olarak kabul edilmez.
    public DeleteTodoCommandValidator() => RuleFor(command => command.Id).GreaterThan(0);
}

/// <summary>
/// Silinecek entity'yi bulur ve repository aracılığıyla kalıcı silme işlemini yürütür.
/// </summary>
public sealed class DeleteTodoCommandHandler(
    ITodoRepository todoRepository, 
    ITodoNotificationService notificationService,
    IRoomMemberRepository roomMemberRepository) : IRequestHandler<DeleteTodoCommand>
{
    public async Task Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        // Silmeden once entity bulunur; yoksa silme yerine anlamli bir hata uretilir.
        var todo = await todoRepository.GetByIdAsync(request.Id, request.UserId, cancellationToken)
            ?? throw new NotFoundException("Todo", request.Id);

        // RBAC: Odada Owner rolü olmayan veya görevi oluşturmayan kişilerin silmesi engellenir.
        if (todo.RoomId.HasValue && todo.UserId != request.UserId)
        {
            var roomMember = await roomMemberRepository.GetAsync(todo.RoomId.Value, request.UserId, cancellationToken);
            if (roomMember == null || roomMember.Role != TodoApp.Domain.Enums.Role.Owner)
            {
                throw new Exception("Yalnızca görevi oluşturan kişi veya Oda Sahibi bu görevi silebilir.");
            }
        }

        // Silme once EF Core tarafinda isaretlenir, sonra SaveChanges ile kalici olur.
        await todoRepository.DeleteAsync(todo, cancellationToken);
        await todoRepository.SaveChangesAsync(cancellationToken);

        if (todo.RoomId.HasValue)
        {
            await notificationService.NotifyTaskUpdatedAsync(todo.RoomId.Value, cancellationToken);
        }
    }
}
