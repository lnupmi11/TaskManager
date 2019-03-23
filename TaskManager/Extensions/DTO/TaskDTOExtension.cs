using System;
using TaskManager.DTO.Task;
using TaskItem = TaskManager.DAL.Models.TaskItem;

namespace TaskManager.Extensions
{
    public static class TaskDTOExtension
    {
        public static TaskItem ToTask(this TaskItemDTO taskDto)
        {
            return new TaskItem
            {
                Id = taskDto.Id,
                Name = taskDto.Name,
                Description = taskDto.Description,
                ElapsedTime = TimeSpan.FromMilliseconds(taskDto.ElapsedTime),
                Goal = taskDto.Goal,
                LastStartTime = taskDto.LastStartTime,
                IsRunning = taskDto.IsRunning,
                UserId = taskDto.UserId,
                WatchType = taskDto.WatchType
            };
        }
        
        public static TaskItemDTO ToTaskDto(this TaskItem task)
        {
            return new TaskItemDTO
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                ElapsedTime = (int)task.ElapsedTime.TotalMilliseconds,
                Goal = task.Goal,
                LastStartTime = task.LastStartTime,
                IsRunning = task.IsRunning,
                UserId = task.UserId,
                WatchType = task.WatchType
            };
        }
    }
}
