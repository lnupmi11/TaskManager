using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TaskManager.BLL.Interfaces;
using TaskManager.DAL.Models.Enums;
using TaskManager.DTO.Task;
using TaskManager.Extensions.Auth;

namespace TaskManager.Controllers
{
    [AuthAttributeExtension(Roles.User)]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: Task
        public IActionResult Index(List<Priority> priorities, Category? category)
        {
            ViewBag.Priorities = priorities;
            ViewBag.Category = category;

            var tasks = _taskService.GetAllByFilters(priorities, category);
            return View(tasks);
        }

        // GET: Task/Details/{id}
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItemDTO = _taskService.Find(id);
            if (taskItemDTO == null)
            {
                return NotFound();
            }

            return View(taskItemDTO);
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
                _taskService.Create(User, taskItemDTO);

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

            var taskItemDTO = _taskService.Find(id);
            if (taskItemDTO == null)
            {
                return NotFound();
            }

            return View(taskItemDTO);
        }

        // POST: Task/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("Id,Title,Description,EstimatedTime,Progress,StartDate,EndDate,Category,Priority,Status,UserId")] TaskItemDTO taskItemDTO)
        {
            if (id != taskItemDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _taskService.Update(taskItemDTO);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_taskService.Any(taskItemDTO.Id))
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

            var taskItemDTO = _taskService.Find(id);
            if (taskItemDTO == null)
            {
                return NotFound();
            }
            
            return View(taskItemDTO);
        }

        // POST: Task/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            if(!_taskService.Any(id))
            {
                return NotFound();
            }

            _taskService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
