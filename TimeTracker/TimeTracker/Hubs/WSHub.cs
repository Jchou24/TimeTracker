using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TimeTracker.Hubs;

namespace ChatSample.Hubs
{
    public class WSHub : Hub
    {

        private readonly WSHubHandler<WSHub> _hubMethods;

        public WSHub(WSHubHandler<WSHub> hubMethods)
        {
            _hubMethods = hubMethods;
        }

        [Authorize]
        public async Task Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastMessage", name, message);
        }

        
    }
}