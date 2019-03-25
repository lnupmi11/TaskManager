using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.BLL.Interfaces;

namespace TaskManager.Controllers
{
    [Route("task")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskManager _taskManager;

        public TaskController(ITaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        [HttpGet("getAll")]
        public IActionResult GetAllTasks()
        {
            var allTasks = _taskManager.GetAllTasks();

            return Ok(allTasks);
        }
    }
}
