using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.DAL.Models.Enums;

namespace TaskManager.DAL.Models
{
    public class TaskItem
    {
        public TaskItem()
        {
            Changes = new List<TaskChanges>();
        }

        [Key]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public TimeSpan EstimatedTime { get; set; }

        public int? Progress { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Category Category { get; set; }

        public Priority Priority { get; set; }

        public Status Status { get; set; }

        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; }

        public virtual UserProfile User { get; set; }

        public virtual ICollection<TaskChanges> Changes { get; set; }
    }
}
