using System.Collections.Generic;
using TaskManager.BLL.Interfaces;
using TaskManager.DAL.Interfaces;
using TaskManager.DAL.Models;

namespace TaskManager.BLL.Services
{
    public class TaskService : ITaskService
    {
        private IRepository<TaskItem> _taskRepository;

        public TaskService(IRepository<TaskItem> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public virtual IEnumerable<TaskItem> GetAll()
        {
            return _taskRepository.GetAll();
        }

        public virtual void Create(TaskItem task)
        {
            _taskRepository.Create(task);
        }

        public virtual TaskItem Find(string id)
        {
            return _taskRepository.Find(id);
        }

        public virtual void Delete(TaskItem task)
        {
            _taskRepository.Delete(task);
        }

        public virtual void Update(TaskItem task)
        {
            _taskRepository.Update(task);
        }

        public virtual bool Any(string id)
        {
            return _taskRepository.Any(e => e.Id == id);
        }
    }
}
