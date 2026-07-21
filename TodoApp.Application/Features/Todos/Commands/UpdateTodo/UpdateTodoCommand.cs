using FluentValidation;
using MediatR;
using TodoApp.Application.Common.Exceptions;
using TodoApp.Application.Abstractions.Persistence;
using TodoApp.Domain.Enums;

namespace TodoApp.Application.Features.Todos.Commands.UpdateTodo;

/// <summary>
/// Var olan bir Todo'nun değiştirilebilir alanlarını güncelleme command'idir.
/// </summary>
public sealed record UpdateTodoCommand(int Id, string Title, string? Description, Priority Priority, DateTime? DueDate) : IRequest;

/// <summary>
/// Güncelleme isteğinin biçim kurallarını handler'dan ayırır.
/// </summary>
public sealed class UpdateTodoCommandValidator : AbstractValidator<UpdateTodoCommand>
{
    public UpdateTodoCommandValidator()
    {
        RuleFor(command => command.Id).GreaterThan(0);
        RuleFor(command => command.Title).NotEmpty().MaximumLength(200);
        RuleFor(command => command.Description).MaximumLength(2000);
        RuleFor(command => command.Priority).IsInEnum();
    }
}

/// <summary>
/// Kayıt bulunması, alanlarının değiştirilmesi ve kaydedilmesinden sorumludur.
/// </summary>
public sealed class UpdateTodoCommandHandler(ITodoRepository todoRepository) : IRequestHandler<UpdateTodoCommand>
{
    public async Task Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Todo", request.Id);

        todo.UpdateDetails(request.Title, request.Description, request.Priority, request.DueDate);

        await todoRepository.UpdateAsync(todo, cancellationToken);
        await todoRepository.SaveChangesAsync(cancellationToken);
    }
}
