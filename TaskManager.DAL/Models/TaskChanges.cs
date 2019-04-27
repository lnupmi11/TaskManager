using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.DAL.Models
{
    public class TaskChanges
    {
        public string Id { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string Description { get; set; }

        [ForeignKey("TaskItem")]
        public string TaskId { get; set; }

        public virtual TaskItem Task { get; set; }
    }
}
