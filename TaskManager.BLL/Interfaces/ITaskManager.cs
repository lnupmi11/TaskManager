using System.Collections.Generic;
using TaskManager.DTO.Task;

namespace TaskManager.BLL.Interfaces
{
    public interface ITaskManager : IBaseManager
    {
        IEnumerable<TaskItemDTO> GetAllTasks();
    }
}
