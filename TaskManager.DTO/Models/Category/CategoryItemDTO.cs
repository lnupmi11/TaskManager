using System.ComponentModel.DataAnnotations;

namespace TaskManager.DTO.Models.Category
{
    public class CategoryItemDTO
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public string UserId { get; set; }
    }
}
