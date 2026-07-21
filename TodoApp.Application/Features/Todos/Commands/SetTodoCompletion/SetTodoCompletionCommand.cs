using FluentValidation;
using MediatR;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Domain.Enums;

namespace TodoApp.Application.Features.Todos.Commands.UpdateTodoStatus;

public sealed record UpdateTodoStatusCommand(int Id, TodoStatus Status) : IRequest;

public sealed class UpdateTodoStatusCommandValidator : AbstractValidator<UpdateTodoStatusCommand>
{
    public UpdateTodoStatusCommandValidator()
    {
        RuleFor(command => command.Id).GreaterThan(0);
        RuleFor(command => command.Status).IsInEnum();
    }
}

public sealed class UpdateTodoStatusCommandHandler(ITodoRepository todoRepository)
    : IRequestHandler<UpdateTodoStatusCommand>
{
    public async Task Handle(UpdateTodoStatusCommand request, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Todo", request.Id);

        todo.SetStatus(request.Status);
        await todoRepository.UpdateAsync(todo, cancellationToken);
        await todoRepository.SaveChangesAsync(cancellationToken);
    }
}
