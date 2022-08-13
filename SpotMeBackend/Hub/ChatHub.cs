using Microsoft.AspNetCore.SignalR;

namespace SpotMeBackend.Hub;

public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
{
    public Task SendMessage1(string user, string message)               // Two parameters accepted
    {
        return Clients.All.SendAsync("ReceiveOne", user, message);    // Note this 'ReceiveOne' 
    }
    
}