using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
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
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "create")
                .Options;

            var context = new ApplicationDbContext(options);

            var repository = new TaskRepository(context);

            var userRep = new Mock<UserRepository>(context);

            var mapper = new Mock<IMapper>();
            var task = new TaskItemDTO { Id = "1", Description = "Description", UserId = "1" };
            var taskItem = new TaskItem { Id = "1", Description = "Description", UserId = "1" };
            mapper.Setup(x => x.Map<TaskItem>(task)).Returns(taskItem);

            var userService = new Mock<UserService>(userRep.Object);

            var service = new TaskService(repository,userService.Object,mapper.Object);
                  
            var controller = new TaskController(service);
            // Act
            var view=controller.Create(task);

            // Assert
            Assert.Equal(1, context.Tasks.Count());
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

            var repository = new TaskRepository(context);

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

            var userService = new Mock<UserService>(userRep.Object);

            var service = new TaskService(repository, userService.Object, mapper.Object);

            var controller = new TaskController(service);
            // Act
            controller.Create(task);
            task.Description = "new";
            var view = controller.Edit(task.Id,task);
            var str = view.ToString();
            // Assert
            Assert.Equal(1, context.Tasks.Count());
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

            var userRep = new Mock<UserRepository>(context);

            var mapper = new Mock<IMapper>();
            var task = new TaskItemDTO { Id = "1", Description = "Description", UserId = "1" };
            var taskItem = new TaskItem { Id = "1", Description = "Description", UserId = "1" };
            mapper.Setup(x => x.Map<TaskItem>(task)).Returns(taskItem);
            mapper.Setup(x => x.Map<TaskItemDTO>(taskItem)).Returns(task);

            var userService = new Mock<UserService>(userRep.Object);

            var service = new TaskService(repository, userService.Object, mapper.Object);

            var controller = new TaskController(service);
            // Act
            var view = controller.Create(task);

            var actionResult = controller.Details("1");

            var contentResult = actionResult as ViewResult;

            var okResult = Assert.IsType<ViewResult>(actionResult);

            Assert.Equal("Description", ((controller.Details(task.Id) as ViewResult).Model as TaskItemDTO).Description);

        }

        [Fact]
        public void DeleteConfirmedNotExistingTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "delete1")
                .Options;

            var context = new ApplicationDbContext(options);

            var repository = new TaskRepository(context);

            var userRep = new Mock<UserRepository>(context);

            var mapper = new Mock<IMapper>();
            var task = new TaskItemDTO { Id = "1", Description = "Description", UserId = "1" };
            var taskItem = new TaskItem { Id = "1", Description = "Description", UserId = "1" };
            mapper.Setup(x => x.Map<TaskItem>(task)).Returns(taskItem);
            mapper.Setup(x => x.Map<TaskItemDTO>(taskItem)).Returns(task);

            var userService = new Mock<UserService>(userRep.Object);

            var service = new TaskService(repository, userService.Object, mapper.Object);

            var controller = new TaskController(service);

            // Act
            controller.Create(task);
            var result = controller.Delete("100");

            // Assert
            Assert.Equal(1, context.Tasks.Count());
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

            var userRep = new Mock<UserRepository>(context);

            var mapper = new Mock<IMapper>();
            var task = new TaskItemDTO { Id = "1", Description = "Description", UserId = "1" };
            var taskItem = new TaskItem { Id = "1", Description = "Description", UserId = "1" };
            mapper.Setup(x => x.Map<TaskItem>(task)).Returns(taskItem);
            mapper.Setup(x => x.Map<TaskItemDTO>(taskItem)).Returns(task);

            var userService = new Mock<UserService>(userRep.Object);

            var service = new TaskService(repository, userService.Object, mapper.Object);

            var controller = new TaskController(service);
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

            var userService = new Mock<UserService>(userRep.Object);

            var mapper = new Mock<IMapper>();

            var service = new TaskService(repository.Object, userService.Object, mapper.Object);

            var controller = new TaskController(service);
            // Act
            var actionResult = controller.Delete("1");

            // Assert
            Assert.IsType<NotFoundResult>(actionResult);
        }
       
    }
}
