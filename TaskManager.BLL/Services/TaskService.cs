using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using TaskManager.BLL.Extensions.Identity;
using TaskManager.BLL.Interfaces;
using TaskManager.DAL.Interfaces;
using TaskManager.DAL.Models;
using TaskManager.DAL.Models.Enums;
using TaskManager.DTO.Task;

namespace TaskManager.BLL.Services
{
    public class TaskService : ITaskService
    {
        private readonly IRepository<TaskItem> _taskRepository;
        private readonly IRepository<UserProfile> _userRepository;
        private readonly IMapper _mapper;

        public TaskService(IRepository<TaskItem> taskRepository, IRepository<UserProfile> userRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public virtual IEnumerable<TaskItemDTO> GetAll()
        {
            var tasksDTO = _taskRepository.GetAll().Select(task => _mapper.Map<TaskItemDTO>(task)).ToList();

            return tasksDTO;
        }

        public virtual IEnumerable<TaskItemDTO> GetByFilters(List<Priority> priorities, Category? category)
        {
            var tasksDTO = _taskRepository
                .GetAllWhere(x => (!category.HasValue || x.Category == category) &&
                (priorities.Count == 0 || priorities.Contains(x.Priority)))
                .Select(task => _mapper.Map<TaskItemDTO>(task)).ToList();

            return tasksDTO;
        }

        public virtual IEnumerable<TaskItemDTO> GetActiveByFilters(List<Priority> priorities, Category? category)
        {
            var tasksDTO = _taskRepository
                .GetAllWhere(x => (!category.HasValue || x.Category == category) &&
                (priorities.Count == 0 || priorities.Contains(x.Priority)) &&
                (x.Status != Status.Closed))
                .Select(task => _mapper.Map<TaskItemDTO>(task)).ToList();

            return tasksDTO;
        }

        public virtual IEnumerable<TaskItemDTO> GetArchivedByFilters(List<Priority> priorities, Category? category)
        {
            var tasksDTO = _taskRepository
                .GetAllWhere(x => (!category.HasValue || x.Category == category) &&
                (priorities.Count == 0 || priorities.Contains(x.Priority)) &&
                (x.Status == Status.Closed))
                .Select(task => _mapper.Map<TaskItemDTO>(task)).ToList();

            return tasksDTO;
        }


        public virtual void Create(ClaimsPrincipal user, TaskItemDTO taskItemDTO)
        {
            var taskItem = _mapper.Map<TaskItem>(taskItemDTO);
            taskItem.User = _userRepository.Find(user.GetUserId());
            _taskRepository.Create(taskItem);
        }

        public virtual TaskItemDTO Find(string id)
        {
            var taskItem = _taskRepository.Find(id);
            var taskItemDTO = _mapper.Map<TaskItemDTO>(taskItem);

            return taskItemDTO;
        }

        public virtual void Delete(string id)
        {
            _taskRepository.Delete(id);
        }

        public virtual void Update(TaskItemDTO taskItemDTO)
        {
            var taskItem = _mapper.Map<TaskItem>(taskItemDTO);
            var prevTaskItem = _taskRepository.FindAsNoTracking(taskItem.Id);
            var diff = GetDiff(prevTaskItem, taskItem);

            foreach (var item in diff)
            {
                taskItem.Changes.Add(item);
            }

            _taskRepository.Update(taskItem);
        }

        public virtual bool Any(string id)
        {
            return _taskRepository.Any(e => e.Id == id);
        }

        private List<TaskChanges> GetDiff(TaskItem oldTask, TaskItem newTask)
        {
            List<TaskChanges> changes = new List<TaskChanges>();

            var oType = oldTask.GetType();

            foreach (var oProperty in oType.GetProperties())
            {
                var oOldValue = oProperty.GetValue(oldTask, null);
                var oNewValue = oProperty.GetValue(newTask, null);
                if (!Equals(oOldValue, oNewValue) &&
                    oProperty.Name != "TaskChanges" &&
                    oProperty.Name != "User")
                {
                    var sOldValue = oOldValue == null ? "" : oOldValue.ToString();
                    var sNewValue = oNewValue == null ? "" : oNewValue.ToString();

                    changes.Add(new TaskChanges
                    {
                        ModifiedOn = DateTime.Now,
                        Description = "Property " + oProperty.Name + " was changed from \"" + sOldValue + "\" to \"" + sNewValue + "\"",
                        TaskId = newTask.Id
                    });

                }
            }

            return changes;
        }
    }
}
