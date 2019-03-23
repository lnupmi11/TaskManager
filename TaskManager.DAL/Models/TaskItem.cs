using System;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.DAL.Models.Enums;

namespace TaskManager.DAL.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public TimeSpan ElapsedTime { get; set; }

        public DateTime LastStartTime { get; set; }

        public TimeSpan? Goal { get; set; }

        public bool IsRunning { get; set; }

        public int UserId { get; set; }

        public ApplicationUser User { get; set; }

        public WatchType WatchType { get; set; }
    }
}
