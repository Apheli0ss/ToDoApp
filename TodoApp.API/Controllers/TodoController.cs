using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.DTOs;
using TodoApp.Application.Features.Todos.Commands.CreateTodo;
using TodoApp.Application.Features.Todos.Commands.DeleteTodo;
using TodoApp.Application.Features.Todos.Commands.UpdateTodoStatus;
using TodoApp.Application.Features.Todos.Commands.UpdateTodo;
using TodoApp.Application.Features.Todos.Queries.GetTodoById;
using TodoApp.Application.Features.Todos.Queries.GetTodos;
using TodoApp.Application.Features.Comments.Commands.AddComment;
using TodoApp.Application.Features.Comments.Queries.GetComments;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TodoApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public sealed class TodoController(ISender sender) : ControllerBase
{
    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>GET: api/todo - tüm kayıtları query üzerinden getirir.</summary>
    [HttpGet]
    public async Task<ActionResult<TodoApp.Application.Common.Models.PagedResult<TodoDto>>> GetAll(
        [FromQuery] int? roomId, 
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 50, 
        CancellationToken cancellationToken = default)
    {
        var todos = await sender.Send(new GetTodosQuery(GetUserId(), roomId, pageNumber, pageSize), cancellationToken);
        return Ok(todos);
    }

    /// <summary>GET: api/todo/5 - tek kaydı query üzerinden getirir.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TodoDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var todo = await sender.Send(new GetTodoByIdQuery(id, GetUserId()), cancellationToken);
        return Ok(todo);
    }

    /// <summary>POST: api/todo - yeni kaynak oluşturur.</summary>
    [HttpPost]
    public async Task<ActionResult> Create(CreateTodoDto dto, CancellationToken cancellationToken)
    {
        var id = await sender.Send(
            new CreateTodoCommand(GetUserId(), dto.Title, dto.Description, dto.Priority, dto.DueDate, dto.Assignee, dto.Category, dto.EstimatedTime, dto.Subtasks, dto.RoomId), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    /// <summary>PUT: api/todo/5 - Todo'nun düzenlenebilir alanlarını günceller.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateTodoDto dto, CancellationToken cancellationToken)
    {
        await sender.Send(
            new UpdateTodoCommand(id, GetUserId(), dto.Title, dto.Description, dto.Priority, dto.DueDate, dto.Assignee, dto.Category, dto.EstimatedTime, dto.Subtasks), cancellationToken);
        return NoContent();
    }

    /// <summary>DELETE: api/todo/5 - Todo kaydını siler.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteTodoCommand(id, GetUserId()), cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        UpdateTodoStatusDto dto,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new UpdateTodoStatusCommand(id, GetUserId(), dto.Status),
            cancellationToken);

        return NoContent();
    }

    [HttpGet("{id:int}/comments")]
    public async Task<IActionResult> GetComments(int id, CancellationToken cancellationToken)
    {
        var query = new GetCommentsQuery(id);
        var result = await sender.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:int}/comments")]
    public async Task<IActionResult> AddComment(int id, [FromBody] AddCommentRequest request, CancellationToken cancellationToken)
    {
        var command = new AddCommentCommand(id, GetUserId(), request.Content);
        var commentId = await sender.Send(command, cancellationToken);
        return Ok(new { Id = commentId });
    }
}



public class AddCommentRequest
{
    public string Content { get; set; } = string.Empty;
}
