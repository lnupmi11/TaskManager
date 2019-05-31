using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.DAL.Models
{
    public class TaskCategories
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [ForeignKey("Category")]
        public string CategoryId { get; set; }

        public virtual CategoryItem Category { get; set; }

        [Required]
        [ForeignKey("Task")]
        public string TaskId { get; set; }

        public virtual TaskItem Task { get; set; }
    }
}
