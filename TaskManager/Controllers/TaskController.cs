using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.BLL.Interfaces;
using TaskManager.DAL.Models;
using TaskManager.DAL.Models.Enums;

namespace TaskManager.Controllers
{
    [Authorize(Roles = "User")]
    public class TaskController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITaskService _taskService;

        public TaskController(IUserService userService, ITaskService taskService)
        {
            _userService = userService;
            _taskService = taskService;
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

            return View(task);
        }

        // GET: Task/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Task/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,Description,EstimatedTime,Progress,StartDate,EndDate,Category,Priority,Status")] TaskItem taskItem)
        {
            if (ModelState.IsValid)
            {
                taskItem.User = _userService.GetUserProfile(User);
                _taskService.Create(taskItem);

                return RedirectToAction(nameof(Index));
            }

            return View(taskItem);
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

            return View(taskItem);
        }

        // POST: Task/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("Id,Title,Description,EstimatedTime,Progress,StartDate,EndDate,Category,Priority,Status,UserId")] TaskItem taskItem)
        {
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

            return View(taskItem);
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

            return View(taskItem);
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
