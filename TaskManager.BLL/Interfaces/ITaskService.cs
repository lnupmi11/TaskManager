using System.Collections.Generic;
using System.Security.Claims;
using TaskManager.DAL.Models;
using TaskManager.DTO.Task;

namespace TaskManager.BLL.Interfaces
{
    public interface ITaskService
    {
        IEnumerable<TaskItemDTO> GetAll();
        void Create(ClaimsPrincipal user, TaskItemDTO taskItemDTO);
        TaskItemDTO Find(string id);
        void Delete(string id);
        void Update(TaskItemDTO taskItemDTO);
        bool Any(string id);
    }
}
