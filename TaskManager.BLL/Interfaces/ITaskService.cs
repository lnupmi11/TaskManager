using System.Collections.Generic;
using System.Security.Claims;
using TaskManager.DAL.Models.Enums;
using TaskManager.DTO.Task;

namespace TaskManager.BLL.Interfaces
{
    public interface ITaskService
    {
        IEnumerable<TaskItemDTO> GetAll();
        IEnumerable<TaskItemDTO> GetByFilters(List<Priority> priorities, Category? category);
        IEnumerable<TaskItemDTO> GetActiveByFilters(List<Priority> priorities, Category? category);
        IEnumerable<TaskItemDTO> GetArchivedByFilters(List<Priority> priorities, Category? category);

        IEnumerable<TaskItemDTO> GetUserTasks(ClaimsPrincipal principal);
        IEnumerable<TaskItemDTO> GetUserTasksByFilters(ClaimsPrincipal principal, List<Priority> priorities, Category? category);
        IEnumerable<TaskItemDTO> GetUserActiveTasksByFilters(ClaimsPrincipal principal, List<Priority> priorities, Category? category);
        IEnumerable<TaskItemDTO> GetUserArchivedTasksByFilters(ClaimsPrincipal principal, List<Priority> priorities, Category? category);

        void Create(ClaimsPrincipal user, TaskItemDTO taskItemDTO);
        TaskItemDTO Find(string id);
        void Delete(string id);
        void Update(TaskItemDTO taskItemDTO);
        bool Any(string id);
    }
}
