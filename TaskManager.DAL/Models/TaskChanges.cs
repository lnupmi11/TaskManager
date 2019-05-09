using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.DAL.Models
{
    public class TaskChanges
    {
        public string Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ModifiedOn { get; set; }

        public string Description { get; set; }

        [ForeignKey("TaskItem")]
        public string TaskId { get; set; }

        public virtual TaskItem Task { get; set; }
    }
}
