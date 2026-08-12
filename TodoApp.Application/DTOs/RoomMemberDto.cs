using TodoApp.Domain.Enums;

namespace TodoApp.Application.DTOs;

public class RoomMemberDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; }
    public DateTime JoinedAt { get; set; }
}
