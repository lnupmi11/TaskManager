using System;
using Microsoft.AspNetCore.Mvc;
using TaskManager.BLL.Interfaces;
using Microsoft.AspNetCore.Identity;
using TaskManager.DAL.Models;
using TaskManager.Extensions.UI;
using System.Threading.Tasks;
using System.Linq;
using TaskManager.DAL.Models.Enums;
using Microsoft.AspNetCore.Authorization;

namespace TaskManager.Controllers
{
    [Authorize(Roles = "Admin")]
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

        // POST: User/Ban/{id}
        public async Task<IActionResult> Ban(string id)
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

            var lockoutEndDate = new DateTime(2999, 01, 01);
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, lockoutEndDate);

            return RedirectToAction(nameof(Users));
        }

        // POST: User/Ban/{id}
        public async Task<IActionResult> Unban(string id)
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

            await _userManager.SetLockoutEnabledAsync(user, false);

            return RedirectToAction(nameof(Users));
        }
    }
}
