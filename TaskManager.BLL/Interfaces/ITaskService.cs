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
        void Delete(TaskItem task);
        void Update(TaskItem task);
        bool Any(string id);
    }
}
