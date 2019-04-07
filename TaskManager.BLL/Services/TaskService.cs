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

        public IEnumerable<TaskItem> GetAll()
        {
            return _taskRepository.GetAll();
        }

        public void Create(TaskItem task)
        {
            _taskRepository.Create(task);
        }

        public TaskItem Find(string id)
        {
            return _taskRepository.Find(id);
        }

        public void Delete(TaskItem task)
        {
            _taskRepository.Delete(task);
        }

        public void Update(TaskItem task)
        {
            _taskRepository.Update(task);
        }

        public bool Any(string id)
        {
            return _taskRepository.Any(e => e.Id == id);
        }
    }
}
