using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SpotMeBackend.Hub;

namespace SpotMeBackend.Controllers;

[Route("api/chat")]
[ApiController]
public class ChatController  : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;
    
    public ChatController(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    [Route("send")]                                           //path looks like this: https://localhost:44379/api/chat/send
    [HttpPost]
    public async Task SendMessage(string user, string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}