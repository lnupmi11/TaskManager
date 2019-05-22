using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.BLL.Interfaces;
using TaskManager.BLL.Services;
using TaskManager.Controllers;
using TaskManager.DAL.EF;
using TaskManager.DAL.Models;
using TaskManager.DAL.Repositories;
using TaskManager.DTO.Task;
using Xunit;
namespace TaskManager.Tests
{
    public class TaskControllerTests
    {

        [Fact]
        public void CreateTest()
        {
            var context = new ApplicationDbContext();

            var repository = new TaskRepository(context);

            var userRep = new Mock<UserRepository>(context);

            var mapper = new Mock<IMapper>();
            var principal = new Mock<ClaimsPrincipal>();
            principal.Setup(b => b.Identity.IsAuthenticated).Returns(true);
            principal.Setup(b => b.FindFirst(It.IsAny<string>())).Returns(new Claim(ClaimTypes.NameIdentifier, "1"));

            var iden = new Mock<ClaimsIdentity>();
            iden.Setup(i => i.IsAuthenticated).Returns(true);
            iden.Setup(b => b.FindFirst(It.IsAny<string>())).Returns(new Claim(ClaimTypes.NameIdentifier, "1"));
            var task = new TaskItemDTO { Id = "1", Description = "Description", UserId = "1"};
            var taskItem = new TaskItem { Id = "1", Description = "Description", UserId = "1"};
            mapper.Setup(x => x.Map<TaskItem>(task)).Returns(taskItem);

            var userService = new Mock<UserService>(userRep.Object);

            var service = new Mock<TaskService>(repository,userRep.Object,mapper.Object);
            service.Setup(i => i.Create(principal.Object, task));

            var categoryService = new Mock<ICategoryService>();

            var controller = new TaskController(service.Object, categoryService.Object);

            // Act
            var view = controller.Create(task);

            // Assert
            Assert.Equal("Microsoft.AspNetCore.Mvc.RedirectToActionResult", view.ToString());
        }
        [Fact]
        public void UpdateTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "update")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;

            var context = new ApplicationDbContext(options);

            var repository = new Mock<TaskRepository>(context);

            var userRep = new Mock<UserRepository>(context);

            var mapper = new Mock<IMapper>();
            var task = new TaskItemDTO { Id = "1", Description = "Description", UserId = "1" };
            var taskItem = new TaskItem { Id = "1", Description = "Description", UserId = "1" };
            mapper.Setup(x => x.Map<TaskItem>(task)).Returns(taskItem);
            mapper.Setup(x => x.Map<TaskItemDTO>(taskItem)).Returns(task);
            var task1 = new TaskItemDTO { Id = "1", Description = "new", UserId = "1" };
            var taskItem1 = new TaskItem { Id = "1", Description = "new", UserId = "1" };
            mapper.Setup(x => x.Map<TaskItem>(task1)).Returns(taskItem1);
            mapper.Setup(x => x.Map<TaskItemDTO>(taskItem1)).Returns(task1);
            var principal = new Mock<ClaimsPrincipal>();
            principal.Setup(b => b.Identity.IsAuthenticated).Returns(true);
            principal.Setup(b => b.FindFirst(It.IsAny<string>())).Returns(new Claim(ClaimTypes.NameIdentifier, "1"));

            var userService = new Mock<UserService>(userRep.Object);
            repository.Setup(i => i.FindAsNoTracking(It.IsAny<string>())).Returns(taskItem1);
            var service = new Mock<TaskService>(repository.Object, userRep.Object, mapper.Object);
            var categoryService = new Mock<ICategoryService>();
            var controller = new TaskController(service.Object, categoryService.Object);
            // Act
            controller.Create(task);
            task.Description = "new";
            var view = controller.Edit(task.Id,task);
            var str = view.ToString();
            // Assert
            Assert.NotNull(view);
        }

        [Fact]
        public void DetailsTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "details")
                .Options;

            var context = new ApplicationDbContext(options);

            var repository = new TaskRepository(context);

            var userRep = new UserRepository(context);

            var categoryRep = new CategoryRepository(context);

            var taskCategoriesRep = new TaskCategoryRepository(context);

            var mapper = new Mock<IMapper>();
            var task = new TaskItemDTO { Id = "1", Description = "Description", UserId = "1" };
            var taskItem = new TaskItem { Id = "1", Description = "Description", UserId = "1" };
            mapper.Setup(x => x.Map<TaskItem>(task)).Returns(taskItem);
            mapper.Setup(x => x.Map<TaskItemDTO>(taskItem)).Returns(task);
            var principal = new Mock<ClaimsPrincipal>();
            principal.Setup(b => b.Identity.IsAuthenticated).Returns(true);
            principal.Setup(b => b.FindFirst(It.IsAny<string>())).Returns(new Claim(ClaimTypes.NameIdentifier, "1"));

            var userService = new Mock<UserService>(userRep);

            var service = new TaskService(repository, userRep, categoryRep, taskCategoriesRep, mapper.Object);
            var categoryService = new Mock<ICategoryService>();

            var controller = new TaskController(service, categoryService.Object);
            // Act
            var view = controller.Create(task);

            var actionResult = controller.Details("1");

            var contentResult = actionResult as ViewResult;

            var okResult = Assert.IsType<ViewResult>(actionResult);

            Assert.Equal("Description", ((controller.Details(task.Id) as ViewResult).Model as TaskItemDTO).Description);

        }

        [Fact]
        public void DeleteConfirmedTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "delete1")
                .Options;

            var context = new ApplicationDbContext(options);

            var repository = new TaskRepository(context);
            var principal = new Mock<ClaimsPrincipal>();
            principal.Setup(b => b.Identity.IsAuthenticated).Returns(true);
            principal.Setup(b => b.FindFirst(It.IsAny<string>())).Returns(new Claim(ClaimTypes.NameIdentifier, "1"));

            var userRep = new Mock<UserRepository>(context);
            var categoryRep = new Mock<CategoryRepository>(context);
            var taskCategoriesRep = new Mock<TaskCategoryRepository>(context);

            var mapper = new Mock<IMapper>();
            var task = new TaskItemDTO { Id = "1", Description = "Description", UserId = "1" };
            var taskItem = new TaskItem { Id = "1", Description = "Description", UserId = "1" };
            mapper.Setup(x => x.Map<TaskItem>(task)).Returns(taskItem);
            mapper.Setup(x => x.Map<TaskItemDTO>(taskItem)).Returns(task);
   
            var userService = new Mock<UserService>(userRep.Object);

            var service = new TaskService(repository, userRep.Object, categoryRep.Object, taskCategoriesRep.Object, mapper.Object);
            var categoryService = new Mock<ICategoryService>();

            var controller = new TaskController(service, categoryService.Object);
            // Act
            //controller.Create(task);
            controller.DeleteConfirmed("1");

            // Assert
            Assert.Equal(0, context.Tasks.Count());

        }

        [Fact]
        public void DeleteTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "delete2")
                .Options;

            var context = new ApplicationDbContext(options);

            var repository = new TaskRepository(context);
            var principal = new Mock<ClaimsPrincipal>();
            principal.Setup(b => b.Identity.IsAuthenticated).Returns(true);
            principal.Setup(b => b.FindFirst(It.IsAny<string>())).Returns(new Claim(ClaimTypes.NameIdentifier, "1"));

            var userRep = new Mock<UserRepository>(context);
            var categoryRep = new Mock<CategoryRepository>(context);
            var taskCategoriesRep = new Mock<TaskCategoryRepository>(context);

            var mapper = new Mock<IMapper>();
            var task = new TaskItemDTO { Id = "1", Description = "Description", UserId = "1" };
            var taskItem = new TaskItem { Id = "1", Description = "Description", UserId = "1" };
            mapper.Setup(x => x.Map<TaskItem>(task)).Returns(taskItem);
            mapper.Setup(x => x.Map<TaskItemDTO>(taskItem)).Returns(task);

            var userService = new Mock<UserService>(userRep.Object);
            var categoryService = new Mock<ICategoryService>();

            var service = new TaskService(repository, userRep.Object, categoryRep.Object, taskCategoriesRep.Object, mapper.Object);

            var controller = new TaskController(service, categoryService.Object);
            // Act
            controller.Create(task);
            controller.DeleteConfirmed("1");

            // Assert
            Assert.Equal(0, context.Tasks.Count());
        }
        [Fact]
        public void DeleteNotFoundTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "delete3")
                .Options;

            var context = new ApplicationDbContext(options);

            var repository = new Mock<TaskRepository>(context);

            var userRep = new Mock<UserRepository>(context);
            var categoryRep = new Mock<CategoryRepository>(context);
            var taskCategoriesRep = new Mock<TaskCategoryRepository>(context);

            var userService = new Mock<UserService>(userRep.Object);

            var mapper = new Mock<IMapper>();

            var service = new TaskService(repository.Object, userRep.Object, categoryRep.Object, taskCategoriesRep.Object, mapper.Object);
            var categoryService = new Mock<ICategoryService>();

            var controller = new TaskController(service, categoryService.Object);
            // Act
            var actionResult = controller.Delete("1");

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }
       
    }
}
