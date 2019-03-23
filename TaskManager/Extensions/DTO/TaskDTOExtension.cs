using TaskManager.DAL.Models;
using TaskManager.DTO.Task;

namespace TaskManager.Extensions
{
    public static class TaskDTOExtension
    {
        public static TaskItem ToTask(this TaskItemDTO taskDto)
        {
            return new TaskItem
            {
                Id = taskDto.Id,
                Title = taskDto.Title,
                Description = taskDto.Description,
                EstimatedTime = taskDto.EstimatedTime,
                Progress = taskDto.Progress,
                StartDate = taskDto.StartDate,
                EndDate = taskDto.EndDate,
                Category = taskDto.Category,
                Priority = taskDto.Priority,
                IsActive = taskDto.IsActive,
                UserId = taskDto.UserId
            };
        }
        
        public static TaskItemDTO ToTaskDto(this TaskItem task)
        {
            return new TaskItemDTO
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
                IsActive = task.IsActive,
                UserId = task.UserId
            };
        }
    }
}
