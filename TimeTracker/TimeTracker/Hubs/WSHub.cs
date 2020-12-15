using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTracker.DAL;
using TimeTracker.DAL.Models;
using TimeTracker.Helper.Auth;
using TimeTracker.Hubs;
using TimeTracker.Models.Task;

namespace ChatSample.Hubs
{
    public class WSHub : Hub
    {
        private readonly WSHubHandler<WSHub> _hubMethods;
        private readonly ApplicationDbContext _context;
        private readonly TaskHandler _taskHandler;

        public WSHub(WSHubHandler<WSHub> hubMethods, ApplicationDbContext context, TaskHandler taskHandler)
        {
            _hubMethods = hubMethods;
            this._context = context;
            this._taskHandler = taskHandler;
        }

        [Authorize]
        [AuthorizeRole(UserRoles.User)]
        public async Task Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastMessage", name, message);
            // TODO
            //    測試用 function，要記得刪掉
        }


        [Authorize]
        [AuthorizeRole(UserRoles.User)]
        public async Task DeleteTasks(List<Guid> taskGuids)
        {
            this._taskHandler.DeleteTasks(taskGuids);
            // TODO
            // 要通知 client 誰被刪掉了
        }

        [Authorize]
        [AuthorizeRole(UserRoles.User)]
        public async Task UpdateTaskRowOrder(List<UpdateTaskRowOrder> updateTaskRowOrder)
        {
            var updatedRow = this._taskHandler.UpdateTaskRowOrder(updateTaskRowOrder);
            // TODO
            // 要通知 client 哪些row的order要更新
        }

        [Authorize]
        [AuthorizeRole(UserRoles.User)]
        public async Task UpdateTaskCol(List<UpdateTaskCol> updateTaskCol)
        {
            this._taskHandler.UpdateTaskCol(updateTaskCol);
            // TODO
            // 要通知 client 哪些 cell 要被更新
        }

    }
}