using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManager.BLL.Interfaces;
using TaskManager.DAL.Interfaces;
using TaskManager.DAL.Models;

using TaskManager.DTO.Task;
using TaskManager;
namespace TaskManager.BLL.Managers
{
    public class TaskManager : BaseManager, ITaskManager
    {
        public TaskManager(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IEnumerable<TaskItemDTO> GetAllTasks()
        {
            var tasks = _unitOfWork.TaskItems.GetAll()
                .Select(task => new TaskItemDTO
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    EstimatedTime = task.EstimatedTime,
                    Progress = task.Progress,
                    StartDate = task.StartDate,
                    EndDate = task.EndDate,
                    Category = task.Category,
                    Priority = task.Priority,
                    Status = task.Status,
                    Changes = task.Changes,
                    UserId = task.UserId
                }).ToList();

            return tasks;
        }
    }
}
