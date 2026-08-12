using TodoApp.Domain.Enums;

namespace TodoApp.Domain.Entities;

public class RoomMember
{
    public int RoomId { get; private set; }
    public Room Room { get; private set; } = null!;

    public int UserId { get; private set; }
    public User User { get; private set; } = null!;

    public Role Role { get; private set; }
    public DateTime JoinedAt { get; private set; } = DateTime.UtcNow;

    public RoomMember(int roomId, int userId, Role role)
    {
        RoomId = roomId;
        UserId = userId;
        Role = role;
        JoinedAt = DateTime.UtcNow;
    }

    public void ChangeRole(Role newRole)
    {
        Role = newRole;
    }

    protected RoomMember() { }
}
