using System;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.DAL.Models.Enums;

namespace TaskManager.DAL.Models
{
    public class TaskItem
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

        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
