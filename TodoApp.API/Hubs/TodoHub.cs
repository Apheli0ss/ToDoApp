using Microsoft.AspNetCore.SignalR;

namespace TodoApp.API.Hubs;

public class TodoHub : Hub
{
    // İstemci odaya katıldığında bu metodu çağıracak.
    // SignalR "Groups" özelliği ile istemcileri odalara ayırıyoruz.
    public async Task JoinRoom(string roomCode)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);
    }

    public async Task LeaveRoom(string roomCode)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomCode);
    }
}
