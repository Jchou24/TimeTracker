using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TimeTracker.DAL;
using TimeTracker.Helper.Extensions;
using TimeTracker.Hubs.Models;
using TimeTracker.Models.Task;

namespace TimeTracker.Hubs
{
    public class BaseWSHub : Hub
    {
        protected readonly ApplicationDbContext _context;
        protected readonly TaskHandler _taskHandler;
        protected readonly WSHubHandler<WSHub> _wsHubHandler;

        public BaseWSHub(ApplicationDbContext context, TaskHandler taskHandler, WSHubHandler<WSHub> wsHubHandler)
        {
            this._context = context;
            this._taskHandler = taskHandler;
            this._wsHubHandler = wsHubHandler;
        }

        #region Maintaine Group
        protected async Task RangeAddToTaskEditorGroup(QueryTasks queryTasks)
        {
            foreach (DateTime day in queryTasks.StartDate.EachDay(queryTasks.EndDate))
            {
                await AddToTaskEditorGroup(queryTasks.OwnerGuid, day);
            }
        }

        protected async Task AddToTaskEditorGroup(Guid guid, DateTime date)
        {
            await AddToGroup(_wsHubHandler.GetTaskEditorGroupName(guid, date));
        }

        protected async Task RangeRemoveFromTaskEditorGroup(QueryTasks queryTasks)
        {
            foreach (DateTime day in queryTasks.StartDate.EachDay(queryTasks.EndDate))
            {
                await RemoveFromTaskEditorGroup(queryTasks.OwnerGuid, day);
            }
        }

        protected async Task RemoveFromTaskEditorGroup(Guid guid, DateTime date)
        {
            await RemoveFromGroup(_wsHubHandler.GetTaskEditorGroupName(guid, date));
        }
        #endregion

        #region  Basic Methods
        protected async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        protected async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        protected async Task GroupBroadcast(string groupName, string eventName, object payload)
        {
            await Clients
                //.Groups(groupName, Context.ConnectionId)
                .GroupExcept(groupName, Context.ConnectionId)
                .SendAsync(eventName, payload);
        }
        #endregion
    }
}