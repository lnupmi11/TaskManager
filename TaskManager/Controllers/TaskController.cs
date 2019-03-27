using Microsoft.AspNetCore.Mvc;
using TaskManager.BLL.Managers;
using TaskManager.DAL.EF;
using TaskManager.DAL.Models;
using TaskManager.DAL.Repositories;

namespace TaskManager.Controllers
{
    [Route("task")]
    public class TaskController : ControllerBase
    {
        private readonly TaskManager<TaskItem> _taskManager;
        private readonly WorkContext _context;

        public TaskController(ApplicationDbContext context)
        {
            _context = new WorkContext(context);
            _taskManager = new TaskManager<TaskItem>(_context);
        }

        [HttpGet("listOfAllTasks")]
        public IActionResult GetAllTasks()
        {
            var allTasks = _taskManager.GetAllTasks();

            return Ok(allTasks);
        }
    }
}
