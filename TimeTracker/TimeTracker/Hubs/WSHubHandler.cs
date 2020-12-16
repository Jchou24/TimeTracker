using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using TimeTracker.Hubs.Models;
using TimeTracker.DAL;

namespace TimeTracker.Hubs
{
    public class WSHubHandler<THub> where THub : Hub
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IHubContext<WSHub> _hubContext;

        public WSHubHandler(ApplicationDbContext context, IHubContext<WSHub> hubContext)
        {
            this._context = context;
            this._hubContext = hubContext;
        }

        /// <summary>
        /// ask specific user to get user info by guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public Task ClientGetUserInfo(Guid guid)
        {
            return this._hubContext.Clients.User(guid.ToString()).SendAsync(WSMapCode.GetUserInfo.ToString(), guid.ToString());
        }

        public string GetTaskEditorGroupName(Guid ownerGuid, DateTime date)
        {
            return GetTaskEditorGroupName(ownerGuid, date.ToString("yyyy-MM-dd"));
        }

        public string GetTaskEditorGroupName(Guid ownerGuid, string date)
        {
            return $"TaskEditor+{ownerGuid.ToString()}+{date}";
        }
    }
}
