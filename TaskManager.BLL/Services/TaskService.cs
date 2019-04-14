﻿using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using TaskManager.BLL.Interfaces;
using TaskManager.DAL.Interfaces;
using TaskManager.DAL.Models;
using TaskManager.DTO.Task;

namespace TaskManager.BLL.Services
{
    public class TaskService : ITaskService
    {
        private readonly IRepository<TaskItem> _taskRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public TaskService(IRepository<TaskItem> taskRepository, IUserService userService, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _userService = userService;
            _mapper = mapper;
        }

        public IEnumerable<TaskItemDTO> GetAll()
        {
            var tasksDTO = _taskRepository.GetAll().Select(task => _mapper.Map<TaskItemDTO>(task)).ToList();

            return tasksDTO;
        }

        public void Create(ClaimsPrincipal user, TaskItemDTO taskItemDTO)
        {
            var taskItem = _mapper.Map<TaskItem>(taskItemDTO);
            taskItem.User = _userService.GetUserProfile(user);

            _taskRepository.Create(taskItem);
        }

        public TaskItemDTO Find(string id)
        {
            var taskItem = _taskRepository.Find(id);
            var taskItemDTO = _mapper.Map<TaskItemDTO>(taskItem);

            return taskItemDTO;
        }

        public void Delete(string id)
        {
            _taskRepository.Delete(id);
        }

        public void Update(TaskItemDTO taskItemDTO)
        {
            var taskItem = _mapper.Map<TaskItem>(taskItemDTO);
            _taskRepository.Update(taskItem);
        }

        public bool Any(string id)
        {
            return _taskRepository.Any(e => e.Id == id);
        }
    }
}
