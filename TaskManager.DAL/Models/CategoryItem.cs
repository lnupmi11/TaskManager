using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.DAL.Models
{
    public class CategoryItem
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; }

        public virtual UserProfile User { get; set; }

        public virtual ICollection<TaskCategories> TaskCategories { get; set; }
    }
}
