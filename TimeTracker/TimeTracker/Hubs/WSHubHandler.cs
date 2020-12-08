using ChatSample.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTracker.Hubs.Models;

namespace TimeTracker.Hubs
{
    public class WSHubHandler<THub> where THub : Hub
    {
        private readonly IHubContext<WSHub> _hubContext;

        public WSHubHandler(IHubContext<WSHub> hubContext)
        {
            this._hubContext = hubContext;
        }

        public Task ClientGetUserInfo(Guid guid)
        {
            return _hubContext.Clients.User(guid.ToString()).SendAsync(WSMapCode.GetUserInfo.ToString(), guid.ToString());
        }

        //public Task DirectMessage(Guid guid, string message)
        //{
        //    return _hubContext.Clients.User(guid.ToString()).SendAsync("GetUserInfo", guid.ToString(), Models.WSMapCode.GetUserInfo);
        //}
    }
}
