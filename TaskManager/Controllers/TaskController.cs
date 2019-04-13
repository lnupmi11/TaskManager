using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.BLL.Interfaces;
using TaskManager.DAL.Models;
using TaskManager.DAL.Models.Enums;
using TaskManager.DTO.Task;
using TaskManager.Extensions.Auth;

namespace TaskManager.Controllers
{
    [AuthAttributeExtension(Roles.User)]
    public class TaskController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;

        public TaskController(IUserService userService, ITaskService taskService, IMapper mapper)
        {
            _userService = userService;
            _taskService = taskService;
            _mapper = mapper;
        }

        // GET: Task
        public IActionResult Index()
        {
            var tasks = _taskService.GetAll();
            return View(tasks);
        }

        // GET: Task/Details/{id}
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = _taskService.Find(id);
            if (task == null)
            {
                return NotFound();
            }
            var taskDTO = _mapper.Map<TaskItemDTO>(task);

            return View(taskDTO);
        }

        // GET: Task/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Task/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,Description,EstimatedTime,Progress,StartDate,EndDate,Category,Priority,Status")] TaskItemDTO taskItemDTO)
        {
            if (ModelState.IsValid)
            {
                var taskItem = _mapper.Map<TaskItem>(taskItemDTO);
                taskItem.User = _userService.GetUserProfile(User);
                _taskService.Create(taskItem);

                return RedirectToAction(nameof(Index));
            }

            return View(taskItemDTO);
        }

        // GET: Task/Edit/{id}
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = _taskService.Find(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            var taskItemDTO = _mapper.Map<TaskItemDTO>(taskItem);

            return View(taskItemDTO);
        }

        // POST: Task/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("Id,Title,Description,EstimatedTime,Progress,StartDate,EndDate,Category,Priority,Status,UserId")] TaskItemDTO taskItemDTO)
        {
            var taskItem = _mapper.Map<TaskItem>(taskItemDTO);

            if (id != taskItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _taskService.Update(taskItem);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_taskService.Any(taskItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(taskItemDTO);
        }

        // GET: Task/Delete/{id}
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = _taskService.Find(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            var taskItemDTO = _mapper.Map<TaskItemDTO>(taskItem);
            

            return View(taskItemDTO);
        }

        // POST: Task/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var taskItem = _taskService.Find(id);
            _taskService.Delete(taskItem);

            return RedirectToAction(nameof(Index));
        }
    }
}
