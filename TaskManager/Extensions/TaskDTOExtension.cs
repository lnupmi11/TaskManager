using System;
using TaskManager.DTOModels;
using Task = TaskManager.Models.Task;

namespace TaskManager.Extensions
{
    public static class TaskDTOExtension
    {
        public static Task ToTask(this TaskDTO taskDto)
        {
            return new Task
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
        
        public static TaskDTO ToTaskDto(this Task task)
        {
            return new TaskDTO
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
