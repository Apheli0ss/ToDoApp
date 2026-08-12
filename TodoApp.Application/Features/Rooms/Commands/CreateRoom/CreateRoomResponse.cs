namespace TodoApp.Application.Features.Rooms.Commands.CreateRoom;

public class CreateRoomResponse
{
    public int Id { get; set; }
    public string RoomCode { get; set; } = string.Empty;
}
