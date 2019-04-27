using System.Collections.Generic;
using System.Security.Claims;
using TaskManager.DAL.Models;
using TaskManager.DAL.Models.Enums;
using TaskManager.DTO.Task;

namespace TaskManager.BLL.Interfaces
{
    public interface ITaskService
    {
        IEnumerable<TaskItemDTO> GetAll();
        IEnumerable<TaskItemDTO> GetByFilters(List<Priority> priorities, Category? category);
        void Create(ClaimsPrincipal user, TaskItemDTO taskItemDTO);
        TaskItemDTO Find(string id);
        void Delete(string id);
        void Update(TaskItemDTO taskItemDTO);
        bool Any(string id);
    }
}
