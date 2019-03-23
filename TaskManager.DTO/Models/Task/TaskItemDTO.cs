using System;
using TaskManager.DAL.Models.Enums;

namespace TaskManager.DTO.Task
{
    public class TaskItemDTO
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public TimeSpan EstimatedTime { get; set; }

        public int Progress { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Category Category { get; set; }

        public Priority Priority { get; set; }

        public bool IsActive { get; set; }
        
        public string UserId { get; set; }
    }
}
