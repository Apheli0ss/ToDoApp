using FluentValidation;
using MediatR;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Abstractions.Persistence;

namespace TodoApp.Application.Features.Todos.Commands.DeleteTodo;

/// <summary>
/// Bir Todo kaydını silmek için kullanılan durum değiştiren command'dir.
/// </summary>
public sealed record DeleteTodoCommand(int Id) : IRequest;

/// <summary>
/// Silme command'inin kimlik kuralını tanımlar.
/// </summary>
public sealed class DeleteTodoCommandValidator : AbstractValidator<DeleteTodoCommand>
{
    public DeleteTodoCommandValidator() => RuleFor(command => command.Id).GreaterThan(0);
}

/// <summary>
/// Silinecek entity'yi bulur ve repository aracılığıyla kalıcı silme işlemini yürütür.
/// </summary>
public sealed class DeleteTodoCommandHandler(ITodoRepository todoRepository) : IRequestHandler<DeleteTodoCommand>
{
    public async Task Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Todo", request.Id);

        await todoRepository.DeleteAsync(todo, cancellationToken);
        await todoRepository.SaveChangesAsync(cancellationToken);
    }
}
