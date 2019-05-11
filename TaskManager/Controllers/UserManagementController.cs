using Microsoft.AspNetCore.Mvc;
using TaskManager.BLL.Interfaces;
using Microsoft.AspNetCore.Identity;
using TaskManager.DAL.Models;
using TaskManager.Extensions.UI;
using System.Threading.Tasks;
using System.Linq;
using TaskManager.DAL.Models.Enums;

namespace TaskManager.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;
        private readonly UserManager<UserProfile> _userManager;
        private int _itemsPerPage = 5;

        public UserManagementController(UserManager<UserProfile> userManager, ITaskService taskService, IUserService userService)
        {
            _taskService = taskService;
            _userService = userService;
            _userManager = userManager;
        }

        // GET: Users
        public async Task<IActionResult> Users()
        {
            var ids = (await _userManager.GetUsersInRoleAsync(Roles.User.ToString())).Select(u => u.Id);
            var users =_userService.GetUserProfilesByIds(ids);

            return View(PaginatedList<UserProfile>.Create(users.AsQueryable(), 1, _itemsPerPage));
        }

        // POST: User/Delete/{id}
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _userService.GetUserProfile(id);
            if (user == null)
            {
                return NotFound();
            }

            _userService.Delete(user);

            return RedirectToAction(nameof(Users));
        }
    }
}
