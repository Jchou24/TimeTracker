using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TimeTracker.DAL;
using TimeTracker.DAL.Models;
using TimeTracker.Helper.Auth;
using TimeTracker.Hubs.Models;
using TimeTracker.Models.Task;

namespace TimeTracker.Hubs
{

    [Authorize]
    public class WSHub : BaseWSHub
    {
        public WSHub(ApplicationDbContext context, TaskHandler taskHandler, WSHubHandler<WSHub> wsHubHandler)
            :base(context, taskHandler, wsHubHandler)
        {
        }

        public async Task SubscribeTaskEditor(QueryTasks queryTasks)
        {
            await this.RangeAddToTaskEditorGroup(queryTasks);
        }

        public async Task UnsubscribeTaskEditor(QueryTasks queryTasks)
        {
            await this.RangeRemoveFromTaskEditorGroup(queryTasks);
        }

        public async Task UpdateIsLeave(UpdateIsLeave updateIsLeave)
        {
            await GroupBroadcast(
                _wsHubHandler.GetTaskEditorGroupName(updateIsLeave.OwnerGuid, updateIsLeave.Date),
                WSMapCode.TaskEditorUpdateIsLeave.ToString(),
                updateIsLeave);

            this._taskHandler.CreateOrUpdateTaskDay(updateIsLeave.Date, updateIsLeave.OwnerGuid, updateIsLeave.IsLeave);
        }

        public async Task CreateTask(CreateTask createTask)
        {
            await GroupBroadcast(
                _wsHubHandler.GetTaskEditorGroupName(createTask.OwnerGuid, createTask.Tasks[0].Date),
                WSMapCode.TaskEditorCreateTask.ToString(),
                createTask);
        }

        public async Task DeleteTasks(DeleteTasks deleteTasks)
        {
            await GroupBroadcast(
                _wsHubHandler.GetTaskEditorGroupName(deleteTasks.OwnerGuid, deleteTasks.Date), 
                WSMapCode.TaskEditorDeleteTasks.ToString(), 
                deleteTasks);
            this._taskHandler.DeleteTasks(deleteTasks);
        }

        public async Task UpdateTaskRowOrder(List<UpdateTaskRowOrder> updateTaskRowOrders)
        {
            await GroupBroadcast(
                _wsHubHandler.GetTaskEditorGroupName(updateTaskRowOrders[0].OwnerGuid, updateTaskRowOrders[0].Date),
                WSMapCode.TaskEditorUpdateTaskRowOrder.ToString(),
                updateTaskRowOrders);
            var updatedRow = this._taskHandler.UpdateTaskRowOrder(updateTaskRowOrders);
        }

        public async Task UpdateTaskCol(List<UpdateTaskCol> updateTaskCol)
        {
            await GroupBroadcast(
                _wsHubHandler.GetTaskEditorGroupName(updateTaskCol[0].OwnerGuid, updateTaskCol[0].Date),
                WSMapCode.TaskEditorUpdateTaskCol.ToString(),
                updateTaskCol);
            this._taskHandler.UpdateTaskCol(updateTaskCol);
        }
    }
}