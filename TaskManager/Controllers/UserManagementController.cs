using System;
using Microsoft.AspNetCore.Mvc;
using TaskManager.BLL.Interfaces;
using Microsoft.AspNetCore.Identity;
using TaskManager.DAL.Models;
using System.Threading.Tasks;
using System.Linq;
using TaskManager.DAL.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace TaskManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;
        private readonly UserManager<UserProfile> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly string _infoMessage = "Ban users who are igonorig their tasks and notify them via email";

        private const int BAN_END_YEAR = 3000;
        private const int BAN_END_MONTH = 1;
        private const int BAN_END_DAY = 1;

        private static DateTime _lockoutEndDate = new DateTime(BAN_END_YEAR, BAN_END_MONTH, BAN_END_DAY);

        public UserManagementController(UserManager<UserProfile> userManager, ITaskService taskService,
            IUserService userService, IEmailSender emailSender)
        {
            _taskService = taskService;
            _userService = userService;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // GET: Users
        public IActionResult Users()
        {
            ViewBag.InfoMessage = _infoMessage;
            return View();
        }

        // GET: UsersPartial
        [HttpGet]
        public async Task<PartialViewResult> UsersPartial()
        {
            var ids = (await _userManager.GetUsersInRoleAsync(Roles.User.ToString())).Select(u => u.Id);
            var usersDTO = _userService.GetUsers(ids);

            return PartialView(usersDTO);
        }

        // POST: User/Ban/{id}
        public async Task<IActionResult> Ban(string id)
        {
            var ret = false;

            if (id == null)
            {
                return Json(ret);
            }

            var user = _userService.GetUserProfile(id);
            if (user == null)
            {
                return Json(ret);
            }

            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, _lockoutEndDate);

            ret = true;

            return Json(ret);
        }

        // POST: User/Unban/{id}
        public async Task<IActionResult> Unban(string id)
        {
            var ret = false;
            if (id == null)
            {
                return Json(ret);
            }

            var user = _userService.GetUserProfile(id);
            if (user == null)
            {
                return Json(ret);
            }

            await _userManager.SetLockoutEnabledAsync(user, false);
            await _userManager.SetLockoutEndDateAsync(user, null);

            ret = true;

            return Json(ret);
        }

        public IActionResult NotifyAccountStatusChanged(string id, bool isBanned)
        {
            var ret = false;
            if (id == null)
            {
                return Json(ret);
            }

            var user = _userService.GetUserProfile(id);
            if (user == null)
            {
                return Json(ret);
            }

            var isAccountLocked = _userService.IsAccountLocked(user);

            ret = true;

            try
            {
                if (isBanned && isAccountLocked)
                {
                    _emailSender.SendEmailAsync(user.Email, "Account lock",
                        "Your account has been locked due to not completing your tasks in time.").Wait();

                }
                else if (!isBanned && !isAccountLocked)
                {
                    _emailSender.SendEmailAsync(user.Email, "Account unlock",
                       "Your account has been unlocked!").Wait();
                }
            }
            catch (Exception)
            {
                ret = false;
            }

            return Json(ret);
        }
    }
}
