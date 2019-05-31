using System.ComponentModel.DataAnnotations;
using System.Linq;
using TaskManager.DTO.Task;

namespace TaskManager.DTO.Models.Task
{
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
        }
    }
}
