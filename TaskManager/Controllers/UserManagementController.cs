using Microsoft.AspNetCore.Mvc;
using TaskManager.BLL.Interfaces;
using Microsoft.AspNetCore.Identity;
using TaskManager.DAL.Models;
using TaskManager.Extensions.UI;
using System.Threading.Tasks;
using System.Linq;

namespace TaskManager.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly UserManager<UserProfile> _userManager;
        private readonly ITaskService _taskService;
        private int _itemsPerPage = 5;

        public UserManagementController(ITaskService taskService, UserManager<UserProfile> userManager)
        {
            _taskService = taskService;
            _userManager = userManager;
        }

        // GET: Users
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.GetUsersInRoleAsync("User");
            return View(PaginatedList<UserProfile>.Create(users.AsQueryable(), 1, _itemsPerPage));
        }

        // POST: User/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Users));
        }

    }
}
