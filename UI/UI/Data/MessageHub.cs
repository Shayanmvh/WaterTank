using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

[AllowAnonymous]
public class MessageHub : Hub
{
    // Send a message to all connected clients
    public async Task SendMessage(string data)
    {
        await Clients.All.SendAsync("ReceiveMessage", data);
    }
}