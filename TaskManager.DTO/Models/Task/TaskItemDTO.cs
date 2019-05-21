using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TaskManager.DAL.Models;
using TaskManager.DAL.Models.Enums;

namespace TaskManager.DTO.Task
{
    public class TaskItemDTO
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public TimeSpan EstimatedTime { get; set; }

        [RangeAttribute(0, 100)]
        public int? Progress { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Categories")]
        public virtual string CategoriesStr { get; set; }

        public Priority Priority { get; set; }

        public Status Status { get; set; }

        public string UserId { get; set; }

        public int? OpenTask { get; set; }

        public int? AllTask { get; set; }

        public ICollection<TaskChanges> Changes { get; set; }

        public ICollection<TaskCategories> Categories { get; set; }
    }

    public class TaskItemDTOResponse : TaskItemDTO
    {
        [Display(Name = "Categories")]
        public override string CategoriesStr
        {
            get
            {
                return Categories == null ?
                    string.Empty
                    : string.Join(", ", Categories.Select(_ => _.Category.Name));
            }
            set
            {
                
            }
        }
    }
}
