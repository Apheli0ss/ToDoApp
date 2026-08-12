namespace TodoApp.Application.DTOs;

public class RoomDto
{
    public int Id { get; set; }
    public string RoomCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public List<RoomMemberDto> Members { get; set; } = new();
    public List<TodoDto> Todos { get; set; } = new();
}
