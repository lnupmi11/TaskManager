using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TaskManager.BLL.Interfaces;
using Microsoft.AspNetCore.Identity;
using TaskManager.DAL.Models;
using TaskManager.Extensions.UI;
using System.Threading.Tasks;
using System.Linq;
using TaskManager.DAL.Models.Enums;
using TaskManager.DTO.Models.UserManagement;
using Microsoft.AspNetCore.Authorization;

namespace TaskManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;
        private readonly UserManager<UserProfile> _userManager;
        private readonly int _itemsPerPage = 5;
        private readonly string _infoMessage = "Ban users who are igonorig their tasks and notify them via email";

        public UserManagementController(UserManager<UserProfile> userManager, ITaskService taskService, IUserService userService)
        {
            _taskService = taskService;
            _userService = userService;
            _userManager = userManager;
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

            var lockoutEndDate = new DateTime(2999, 01, 01);
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, lockoutEndDate);

            ret = true;

            return Json(ret);
        }

        // POST: User/Ban/{id}
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

            ret = true;

            return Json(ret);
        }

        public async Task<IActionResult> NotifyAccountStatusChanged(string id, bool isBanned)
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

            // if isBanned == true send msg that account banned, otherwise - account unclocked

            // TODO : send an email that account has been banned also check if account is really banned

            return Json(ret);
        }
    }
}
