using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApp.Application.DTOs;
using TodoApp.Application.Features.Rooms.Commands.CreateRoom;
using TodoApp.Application.Features.Rooms.Commands.JoinRoom;
using TodoApp.Application.Features.Rooms.Commands.LeaveRoom;
using TodoApp.Application.Features.Rooms.Commands.DeleteRoom;
using TodoApp.Application.Features.Rooms.Queries.GetRoomByCode;
using TodoApp.Application.Features.Todos.Queries.GetTodos;

namespace TodoApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    private readonly ISender _sender;

    public RoomController(ISender sender)
    {
        _sender = sender;
    }

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost("create")]
    public async Task<ActionResult<RoomDto>> Create([FromBody] CreateRoomDto dto, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new CreateRoomCommand(GetUserId(), dto), cancellationToken);
        return Ok(result);
    }

    [HttpPost("join")]
    public async Task<ActionResult<RoomDto>> Join([FromBody] JoinRoomDto dto, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new JoinRoomCommand(GetUserId(), dto), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{code}")]
    public async Task<ActionResult<RoomDto>> GetByCode(string code, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetRoomByCodeQuery(GetUserId(), code), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{code}/todos")]
    public async Task<ActionResult> GetRoomTodos(string code, CancellationToken cancellationToken)
    {
        // First get the room to verify access and get RoomId
        var room = await _sender.Send(new GetRoomByCodeQuery(GetUserId(), code), cancellationToken);
        
        // Use existing GetTodosQuery by passing RoomId
        var todos = await _sender.Send(new GetTodosQuery(GetUserId(), room.Id), cancellationToken);
        return Ok(todos);
    }

    [HttpPost("{code}/leave")]
    public async Task<ActionResult> Leave(string code, CancellationToken cancellationToken)
    {
        await _sender.Send(new LeaveRoomCommand(GetUserId(), code), cancellationToken);
        return NoContent();
    }

    [HttpDelete("{code}")]
    public async Task<ActionResult> Delete(string code, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteRoomCommand(GetUserId(), code), cancellationToken);
        return NoContent();
    }
}
