using FluentValidation;
using MediatR;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Enums;

namespace TodoApp.Application.Features.Todos.Commands.CreateTodo;

public sealed record CreateTodoCommand(
    string Title,
    string? Description,
    Priority Priority,
    DateTime? DueDate) : IRequest<int>;

public sealed class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoCommandValidator()
    {
        RuleFor(command => command.Title).NotEmpty().MaximumLength(200);
        RuleFor(command => command.Description).MaximumLength(2000);
        RuleFor(command => command.Priority).IsInEnum();
    }
}

public sealed class CreateTodoCommandHandler(ITodoRepository todoRepository)
    : IRequestHandler<CreateTodoCommand, int>
{
    public async Task<int> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = new TodoItem(request.Title, request.Description, request.Priority, request.DueDate);
        await todoRepository.AddAsync(todo, cancellationToken);
        await todoRepository.SaveChangesAsync(cancellationToken);
        return todo.Id;
    }
}
