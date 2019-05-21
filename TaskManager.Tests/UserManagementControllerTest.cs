using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using TaskManager.BLL.Services;
using TaskManager.Controllers;
using TaskManager.DAL.EF;
using TaskManager.DAL.Models;
using TaskManager.DAL.Repositories;
using TaskManager.DTO.Task;
using TaskManager.Extensions.Email;
using Xunit;
using System.Threading.Tasks;
using TaskManager.DAL.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
namespace TaskManager.Tests
{
    public class UserManagementControllerTest
    {
        [Fact]
        public void UsersViewTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "users")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;

            var context = new ApplicationDbContext(options);

            var emailSender = new Mock<IEmailSender>();
            var taskRep = new Mock<TaskRepository>(context);
            var userRep = new Mock<UserRepository>(context);
            var mapper = new Mock<IMapper>();
            var userService = new Mock<UserService>(userRep.Object,mapper.Object);
            var service = new Mock<TaskService>(taskRep.Object, userRep.Object, mapper.Object);

            var mockUserManager = new Mock<UserManager<UserProfile>>(
                    new Mock<IUserStore<UserProfile>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<UserProfile>>().Object,
                    new IUserValidator<UserProfile>[0],
                    new IPasswordValidator<UserProfile>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<UserProfile>>>().Object);

            var controller = new UserManagementController(mockUserManager.Object, service.Object, userService.Object, emailSender.Object);

            var view = controller.Users();

            Assert.IsType<ViewResult>(view);
        }

        [Fact]
        public void BanTest()
        {
            var user = new UserProfile { Id = "1", UserName = "aaaa",LockoutEnabled=false };
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "users")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;

            var context = new ApplicationDbContext(options);

            var emailSender = new Mock<IEmailSender>();
            var taskRep = new Mock<TaskRepository>(context);
            var userRep = new Mock<UserRepository>(context);
            var mapper = new Mock<IMapper>();
            var userService = new Mock<UserService>(userRep.Object, mapper.Object);
            userService.Setup(i => i.GetUserProfile("1")).Returns(user);

            var service = new Mock<TaskService>(taskRep.Object, userRep.Object, mapper.Object);

            var mockUserManager = new Mock<UserManager<UserProfile>>(
                    new Mock<IUserStore<UserProfile>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<UserProfile>>().Object,
                    new IUserValidator<UserProfile>[0],
                    new IPasswordValidator<UserProfile>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<UserProfile>>>().Object);

            var controller = new UserManagementController(mockUserManager.Object, service.Object, userService.Object, emailSender.Object);

            var view = controller.Ban("1");

            Assert.IsType<JsonResult>(view);

        }

        [Fact]
        public void UnBanTest()
        {
            var user = new UserProfile { Id = "1", UserName = "aaaa",LockoutEnabled=true };
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "users")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;

            var context = new ApplicationDbContext(options);

            var emailSender = new Mock<IEmailSender>();
            var taskRep = new Mock<TaskRepository>(context);
            var userRep = new Mock<UserRepository>(context);
            var mapper = new Mock<IMapper>();
            var userService = new Mock<UserService>(userRep.Object, mapper.Object);
            userService.Setup(i => i.GetUserProfile("1")).Returns(user);

            var service = new Mock<TaskService>(taskRep.Object, userRep.Object, mapper.Object);

            var mockUserManager = new Mock<UserManager<UserProfile>>(
                    new Mock<IUserStore<UserProfile>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<UserProfile>>().Object,
                    new IUserValidator<UserProfile>[0],
                    new IPasswordValidator<UserProfile>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<UserProfile>>>().Object);

            var controller = new UserManagementController(mockUserManager.Object, service.Object, userService.Object, emailSender.Object);

            var view = controller.Unban("1");

            Assert.IsType<JsonResult>(view);

        }
    }
}
