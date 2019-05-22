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
        private readonly IRepository<CategoryItem> _categoryRepository;
        private readonly IRepository<TaskCategories> _taskCategoryRepository;
        private readonly IMapper _mapper;

        public TaskService(IRepository<TaskItem> taskRepository, IRepository<UserProfile> userRepository,
                            IRepository<CategoryItem> categoryRepository, IRepository<TaskCategories> taskCategoryRepository,
                            IMapper mapper)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _taskCategoryRepository = taskCategoryRepository;
            _mapper = mapper;
        }

        public virtual IEnumerable<TaskItemDTO> GetAll()
        {
            var tasksDTO = _taskRepository
                .GetAll()
                .Select(task => _mapper.Map<TaskItemDTOResponse>(task))
                .ToList();

            return tasksDTO;
        }

        public virtual IEnumerable<TaskItemDTO> GetUserTasks(ClaimsPrincipal principal)
        {
            var tasksDTO = _taskRepository
                .GetAllWhere(p => p.UserId == principal.GetUserId())
                .Select(task => _mapper.Map<TaskItemDTOResponse>(task))
                .ToList();

            return tasksDTO;
        }

        public virtual IEnumerable<TaskItemDTO> GetByFilters(List<Priority> priorities, List<string> categories)
        {
            var tasksDTO = _taskRepository
                .GetAllWhere(x => (categories.Count == 0 || x.Categories.Any(c => categories.Contains(c.CategoryId))) &&
                (priorities.Count == 0 || priorities.Contains(x.Priority)))
                .Select(task => _mapper.Map<TaskItemDTOResponse>(task))
                .ToList();

            return tasksDTO;
        }

        public virtual IEnumerable<TaskItemDTO> GetUserTasksByFilters(ClaimsPrincipal principal, List<Priority> priorities, List<string> categories)
        {
            var tasksDTO = _taskRepository
                .GetAllWhere(x => (x.UserId == principal.GetUserId()) &&
                (categories.Count == 0 || x.Categories.Any(c => categories.Contains(c.CategoryId))) &&
                (priorities.Count == 0 || priorities.Contains(x.Priority)))
                .Select(task => _mapper.Map<TaskItemDTOResponse>(task))
                .ToList();

            return tasksDTO;
        }

        public virtual IEnumerable<TaskItemDTO> GetActiveByFilters(List<Priority> priorities, List<string> categories)
        {
            var tasksDTO = _taskRepository
                .GetAllWhere(x => (categories.Count == 0 || x.Categories.Any(c => categories.Contains(c.CategoryId))) &&
                (priorities.Count == 0 || priorities.Contains(x.Priority)) &&
                (x.Status != Status.Closed))
                .Select(task => _mapper.Map<TaskItemDTOResponse>(task))
                .ToList();

            return tasksDTO;
        }

        public virtual IEnumerable<TaskItemDTO> GetUserActiveTasksByFilters(ClaimsPrincipal principal, List<Priority> priorities, List<string> categories)
        {
            var tasksDTO = _taskRepository
                .GetAllWhere(x => (x.UserId == principal.GetUserId()) &&
                (categories.Count == 0 || x.Categories.Any(c => categories.Contains(c.CategoryId))) &&
                (priorities.Count == 0 || priorities.Contains(x.Priority)) &&
                (x.Status != Status.Closed))
                .Select(task => _mapper.Map<TaskItemDTOResponse>(task))
                .ToList();

            return tasksDTO;
        }

        public virtual IEnumerable<TaskItemDTO> GetArchivedByFilters(List<Priority> priorities, List<string> categories)
        {
            var tasksDTO = _taskRepository
                .GetAllWhere(x => (categories.Count == 0 || x.Categories.Any(c => categories.Contains(c.CategoryId))) &&
                (priorities.Count == 0 || priorities.Contains(x.Priority)) &&
                (x.Status == Status.Closed))
                .Select(task => _mapper.Map<TaskItemDTOResponse>(task))
                .ToList();

            return tasksDTO;
        }

        public virtual IEnumerable<TaskItemDTO> GetUserArchivedTasksByFilters(ClaimsPrincipal principal, List<Priority> priorities, List<string> categories)
        {
            var tasksDTO = _taskRepository
                .GetAllWhere(x => (x.UserId == principal.GetUserId()) &&
                (categories.Count == 0 || x.Categories.Any(c => categories.Contains(c.CategoryId))) &&
                (priorities.Count == 0 || priorities.Contains(x.Priority)) &&
                (x.Status == Status.Closed))
                .Select(task => _mapper.Map<TaskItemDTOResponse>(task))
                .ToList();

            return tasksDTO;
        }

        public virtual void Create(ClaimsPrincipal user, TaskItemDTO taskItemDTO)
        {
            var categoriesList = taskItemDTO.CategoriesStr.Split(',').ToList();
            var categories = SyncCategories(categoriesList, user.GetUserId());

            var taskItem = _mapper.Map<TaskItem>(taskItemDTO);
            taskItem.User = _userRepository.Find(user.GetUserId());
            _taskRepository.Create(taskItem);

            CreateTaskCategories(categories, taskItem.Id);
        }

        public virtual TaskItemDTO Find(string id)
        {
            var taskItem = _taskRepository.Find(id);
            var taskItemDTO = _mapper.Map<TaskItemDTOResponse>(taskItem);
            taskItemDTO.Categories = _taskCategoryRepository.GetAllWhere(c => c.TaskId == id).ToList();

            return taskItemDTO;
        }

        public virtual void Delete(string id)
        {
            DeleteTaskCategoreis(id);
            _taskRepository.Delete(id);
        }

        public virtual void Update(TaskItemDTO taskItemDTO)
        {
            CreateUpdateTaskCategories(taskItemDTO.UserId, taskItemDTO);

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
                    oProperty.Name != "Changes" &&
                    oProperty.Name != "User")
                {
                    var sOldValue = oOldValue == null ? "" : oOldValue.ToString();
                    var sNewValue = oNewValue == null ? "" : oNewValue.ToString();
                    var date = DateTime.Now;
                    changes.Add(new TaskChanges
                    {
                        ModifiedOn = date,
                        Description = oProperty.Name + " was changed from \"" + sOldValue + "\" to \"" + sNewValue + "\"",
                        TaskId = newTask.Id
                    });

                }
            }

            return changes;
        }

        private List<CategoryItem> SyncCategories(List<string> categories, string userId)
        {
            var taskCategories = new List<CategoryItem>();
            foreach (var item in categories)
            {
                var category = _categoryRepository.Find(c => c.Name == item);
                if (category != null)
                {
                    taskCategories.Add(category);
                }
                else
                {
                    var newCategory = new CategoryItem { Name = item, UserId = userId };
                    _categoryRepository.Create(newCategory);
                    taskCategories.Add(newCategory);
                }
            }

            return taskCategories;
        }

        private void CreateTaskCategories(List<CategoryItem> categories, string taskId)
        {
            foreach (var category in categories)
            {
                var taskCategory = new TaskCategories { CategoryId = category.Id, TaskId = taskId };
                _taskCategoryRepository.Create(taskCategory);
            }
        }

        private void DeleteTaskCategoreis(string taskId)
        {
            var items = _taskCategoryRepository.GetAllWhere(t => t.TaskId == taskId).ToList();
            foreach (var item in items)
            {
                _taskCategoryRepository.Delete(item.Id);
            }
        }

        private void CreateUpdateTaskCategories(string userId, TaskItemDTO taskItemDTO)
        {
            DeleteTaskCategoreis(taskItemDTO.Id);

            var categoriesList = taskItemDTO.CategoriesStr.Split(',').ToList();
            var categories = SyncCategories(categoriesList, userId);
            CreateTaskCategories(categories, taskItemDTO.Id);
        }
    }
}
