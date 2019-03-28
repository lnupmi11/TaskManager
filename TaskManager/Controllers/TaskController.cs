using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.BLL.Managers;
using TaskManager.DAL.EF;
using TaskManager.DAL.Models;
using TaskManager.DAL.Repositories;

namespace TaskManager.Controllers
{
    [Authorize(Roles = "User")]
    public class TaskController : Controller
    {
        private readonly WorkContext _context;
        private readonly UsersManager _userManager;
        private readonly TasksManager _taskManager;

        public TaskController(ApplicationDbContext context)
        {
            _context = new WorkContext(context);
            _userManager = new UsersManager(_context);
            _taskManager = new TasksManager(_context);
        }

        // GET: Task
        public IActionResult Index()
        {
            var tasks = _taskManager.GetAll();
            return View(tasks);
        }

        // GET: Task/Details/{id}
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = _taskManager.Find(id);
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
        public async Task<IActionResult> Create([Bind("Id,Title,Description,EstimatedTime,Progress,StartDate,EndDate,Category,Priority,Status")] TaskItem taskItem)
        {
            if (ModelState.IsValid)
            {
                taskItem.User = _userManager.GetUserProfile(User);
                await _taskManager.CreateAsync(taskItem);
                return RedirectToAction(nameof(Index));
            }

            return View(taskItem);
        }
    }
}
