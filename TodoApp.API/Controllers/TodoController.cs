using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.DTOs;
using TodoApp.Application.Features.Todos.Commands.CreateTodo;
using TodoApp.Application.Features.Todos.Commands.DeleteTodo;
using TodoApp.Application.Features.Todos.Commands.UpdateTodoStatus;
using TodoApp.Application.Features.Todos.Commands.UpdateTodo;
using TodoApp.Application.Features.Todos.Queries.GetTodoById;
using TodoApp.Application.Features.Todos.Queries.GetTodos;

namespace TodoApp.API.Controllers;

/// <summary>
/// HTTP isteklerini CQRS request'lerine dönüştüren ince API katmanıdır.
/// İş kuralları ve veri erişimi handler/repository katmanlarındadır.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class TodoController(ISender sender) : ControllerBase
{
    /// <summary>GET: api/todo - tüm kayıtları query üzerinden getirir.</summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TodoDto>>> GetAll(CancellationToken cancellationToken)
    {
        var todos = await sender.Send(new GetTodosQuery(), cancellationToken);
        return Ok(todos);
    }

    /// <summary>GET: api/todo/5 - tek kaydı query üzerinden getirir.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TodoDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var todo = await sender.Send(new GetTodoByIdQuery(id), cancellationToken);
        return Ok(todo);
    }

    /// <summary>POST: api/todo - yeni kaynak oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult> Create(CreateTodoDto dto, CancellationToken cancellationToken)
    {
        var id = await sender.Send(
            new CreateTodoCommand(dto.Title, dto.Description, dto.Priority, dto.DueDate), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    /// <summary>PUT: api/todo/5 - Todo'nun düzenlenebilir alanlarını günceller.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateTodoDto dto, CancellationToken cancellationToken)
    {
        await sender.Send(
            new UpdateTodoCommand(id, dto.Title, dto.Description, dto.Priority, dto.DueDate), cancellationToken);
        return NoContent();
    }

    /// <summary>DELETE: api/todo/5 - Todo kaydını siler.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteTodoCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        UpdateTodoStatusDto dto,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new UpdateTodoStatusCommand(id, dto.Status),
            cancellationToken);

        return NoContent();
    }
}
