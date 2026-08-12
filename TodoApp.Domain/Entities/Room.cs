namespace TodoApp.Domain.Entities;

public class Room
{
    public int Id { get; private set; }
    public string RoomCode { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public int CreatedByUserId { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public ICollection<RoomMember> RoomMembers { get; private set; } = new List<RoomMember>();
    public ICollection<TodoItem> Todos { get; private set; } = new List<TodoItem>();

    public Room(string roomCode, string name, int createdByUserId)
    {
        RoomCode = roomCode;
        Name = name;
        CreatedByUserId = createdByUserId;
        CreatedAt = DateTime.UtcNow;
    }

    protected Room() { }
}
