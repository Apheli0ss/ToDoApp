namespace TodoApp.Application.DTOs;

public class RoomSummaryDto
{
    public int Id { get; set; }
    public string RoomCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int MemberCount { get; set; }
    public int TodoCount { get; set; }
}
