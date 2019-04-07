using System;
using System.Collections.Generic;
using System.Text;
using TaskManager.DAL.Models;

namespace TaskManager.BLL.Interfaces
{
    public interface ITaskService
    {
        IEnumerable<TaskItem> GetAll();
        void Create(TaskItem task);
        TaskItem Find(string id);
        void Delete(TaskItem taskItem);
        void Update(TaskItem taskItem);
        bool Any(string id);
    }
}
