using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.BLL.Extensions.Identity;
using TaskManager.BLL.Interfaces;
using TaskManager.DAL.Models.Enums;
using TaskManager.DTO.Task;
using TaskManager.Extensions.Auth;
using TaskManager.Extensions.UI;

namespace TaskManager.Controllers
{
    [AuthAttributeExtension(Roles.User)]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ICategoryService _categoryService;
        private int _itemsPerPage = 5;

        public TaskController(ITaskService taskService, ICategoryService categoryService)
        {
            _taskService = taskService;
            _categoryService = categoryService;
        }

        // GET: Active
        public IActionResult Active(List<Priority> priorities, List<string> categories, int? page)
        {
            SetFilters(priorities, categories, page);
            var tasks = _taskService.GetUserActiveTasksByFilters(User, priorities, categories);

            return View(PaginatedList<TaskItemDTO>.Create(tasks.AsQueryable(), page ?? 1, _itemsPerPage));
        }

        // GET: Archive
        public IActionResult Archive(List<Priority> priorities, List<string> categories, int? page)
        {
            SetFilters(priorities, categories, page);
            var tasks = _taskService.GetUserArchivedTasksByFilters(User, priorities, categories);

            return View(PaginatedList<TaskItemDTO>.Create(tasks.AsQueryable(), page ?? 1, _itemsPerPage));
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
            ViewBag.Categories = _categoryService.GetAllByUserId(User.GetUserId()).Select(c => c.Name).ToList();
            return View();
        }

        // POST: Task/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,Description,EstimatedTime,Progress,StartDate,EndDate,CategoriesStr,Priority,Status")] TaskItemDTO taskItemDTO)
        {
            if (ModelState.IsValid)
            {
                _taskService.Create(User, taskItemDTO);

                return RedirectToAction(nameof(Active));
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

            ViewBag.Categories = _categoryService.GetAllByUserId(User.GetUserId()).Select(c => c.Name).ToList();

            return View(taskItemDTO);
        }

        // POST: Task/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("Id,Title,Description,EstimatedTime,Progress,StartDate,EndDate,CategoriesStr,Priority,Status,UserId")] TaskItemDTO taskItemDTO)
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

                return RedirectToAction(nameof(Active));
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
            if (!_taskService.Any(id))
            {
                return NotFound();
            }

            _taskService.Delete(id);

            return RedirectToAction(nameof(Active));
        }

        #region Helpers

        private void SetFilters(List<Priority> priorities, List<string> categories, int? page)
        {
            ViewBag.Priorities = priorities;
            ViewBag.CategoriesSelected = categories;
            ViewBag.Categories = _categoryService.GetAllByUser(User);
            var open = _taskService.GetUserActiveTasksByFilters(User, priorities, categories).Count();
            ViewBag.OpenTasks = open;
            var all = _taskService.GetUserTasks(User).Count();
            ViewBag.AllTasks = all;
            var closed = _taskService.GetUserArchivedTasksByFilters(User, priorities, categories).Count();
            ViewBag.Closed = closed;
            var progress = (all == 0) ? 0 : closed * 1.0 / all * 100;
            ViewBag.Progress = Math.Round(progress);
        }

        #endregion

    }
}
