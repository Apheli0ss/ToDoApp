using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApp.Application.Features.Notifications.Commands.MarkAsRead;
using TodoApp.Application.Features.Notifications.Queries.GetNotifications;

namespace TodoApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationController(ISender sender) : ControllerBase
{
    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetNotifications(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetNotificationsQuery(GetUserId()), cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id:int}/read")]
    public async Task<IActionResult> MarkAsRead(int id, CancellationToken cancellationToken)
    {
        await sender.Send(new MarkAsReadCommand(id, GetUserId()), cancellationToken);
        return NoContent();
    }
}
