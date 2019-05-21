using System.Collections.Generic;
using System.Security.Claims;
using TaskManager.DTO.Models.Category;

namespace TaskManager.BLL.Interfaces
{
    public interface ICategoryService
    {
        List<CategoryItemDTO> GetAllByUserId(string userId);
        List<CategoryItemDTO> GetAllByUser(ClaimsPrincipal principal);
    }
}
