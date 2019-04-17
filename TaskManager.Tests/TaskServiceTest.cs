using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.BLL.Services;
using TaskManager.DAL.EF;
using TaskManager.DAL.Models;
using TaskManager.DAL.Repositories;
using TaskManager.DTO.Task;
using Xunit;

namespace TaskManager.Tests
{
    public class TaskServiceTest
    {
        [Fact]
        public void GetListTest()
        {
            // Arrange
            var list = GetTestList().AsQueryable();
            var mockSet = new Mock<DbSet<TaskItem>>();
            mockSet.As<IQueryable<TaskItem>>().Setup(m => m.Provider).Returns(list.Provider);
            mockSet.As<IQueryable<TaskItem>>().Setup(m => m.Expression).Returns(list.Expression);
            mockSet.As<IQueryable<TaskItem>>().Setup(m => m.ElementType).Returns(list.ElementType);
            mockSet.As<IQueryable<TaskItem>>().Setup(m => m.GetEnumerator()).Returns(list.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(item => item.Tasks).Returns(mockSet.Object);

            var repository = new TaskRepository(mockContext.Object);

            var userRep = new Mock<UserRepository>(mockContext.Object);

            var mapper = new Mock<IMapper>();
            var task = new TaskItemDTO { Id = "1", Description = "aaaa"};
            var taskItem = new TaskItem { Id = "1", Description = "aaaa"};
            mapper.Setup(x => x.Map<TaskItemDTO>(taskItem)).Returns(task);
            var task1 = new TaskItemDTO { Id = "1", Description = "bbbb" };
            var taskItem1 = new TaskItem { Id = "1", Description = "bbbb" };
            mapper.Setup(x => x.Map<TaskItemDTO>(taskItem1)).Returns(task1);
            var task2 = new TaskItemDTO { Id = "1", Description = "cccc" };
            var taskItem2 = new TaskItem { Id = "1", Description = "cccc" };
            mapper.Setup(x => x.Map<TaskItemDTO>(taskItem2)).Returns(task2);

            var userService = new Mock<UserService>(userRep.Object);

            var service = new TaskService(repository,userService.Object,mapper.Object);

            // Act
            var actual = service.GetAll();

            // Assert
            Assert.Equal(list.Count(), actual.Count());
        }

        [Fact]
        public void CreateTest()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<TaskItem>>();
            var mockContext = new Mock<ApplicationDbContext>();

            mockContext.Setup(item => item.Tasks).Returns(mockDbSet.Object);

            var repository = new Mock<TaskRepository>(mockContext.Object);

            // Act 
            var expTaskDTO = new TaskItemDTO { Id = "4", Description = "ssss" };

            var userRep = new Mock<UserRepository>(mockContext.Object);

            var mapper = new Mock<IMapper>();

            var userService = new Mock<UserService>(userRep.Object);

            var service = new Mock<TaskService>(repository.Object, userService.Object, mapper.Object);

            var cp = new Mock<ClaimsPrincipal>();
            cp.Setup(m => m.HasClaim(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            service.Setup(x => x.Create(cp.Object, expTaskDTO));
            service.Object.Create(cp.Object, expTaskDTO);
            service.Verify(i => i.Create(cp.Object, expTaskDTO));
        }
        [Fact]
        public void UpdateTest()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<TaskItem>>();
            var mockContext = new Mock<ApplicationDbContext>();

            mockContext.Setup(item => item.Tasks).Returns(mockDbSet.Object);

            var repository = new TaskRepository(mockContext.Object);

            var userRep = new Mock<UserRepository>(mockContext.Object);

            var mapper = new Mock<IMapper>();
            var task = new TaskItemDTO { Id = "1", Description = "Description", UserId = "1" };
            var taskItem = new TaskItem { Id = "1", Description = "Description", UserId = "1" };
            mapper.Setup(x => x.Map<TaskItem>(task)).Returns(taskItem);
            mapper.Setup(x => x.Map<TaskItemDTO>(taskItem)).Returns(task);

            var userService = new Mock<UserService>(userRep.Object);

            var service = new TaskService(repository, userService.Object, mapper.Object);
            var cp = new Mock<ClaimsPrincipal>();
            cp.Setup(m => m.HasClaim(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act 
            service.Create(cp.Object, task);
            service.Update(task);
            // Assert
            mockDbSet.Verify(m => m.Update(It.IsAny<TaskItem>()), Times.Once());
        }
        [Fact]
        public void AnyTest()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<TaskItem>>();
            var mockContext = new Mock<ApplicationDbContext>();

            mockContext.Setup(item => item.Tasks).Returns(mockDbSet.Object);

            var repository = new Mock<TaskRepository>(mockContext.Object);

            // Act 
            var exp = new TaskItemDTO { Id = "4", Description = "ssss" };

            var userRep = new Mock<UserRepository>(mockContext.Object);
            var mapper = new Mock<IMapper>();
            var task = new TaskItemDTO { Id = "1", Description = "Description", UserId = "1" };
            var taskItem = new TaskItem { Id = "1", Description = "Description", UserId = "1" };
            mapper.Setup(x => x.Map<TaskItem>(task)).Returns(taskItem);
            mapper.Setup(x => x.Map<TaskItemDTO>(taskItem)).Returns(task);
            var userService = new Mock<UserService>(userRep.Object);

            var service = new Mock<TaskService>(repository.Object, userService.Object, mapper.Object);
            var cp = new Mock<ClaimsPrincipal>();
            cp.Setup(m => m.HasClaim(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            service.Setup(x => x.Create(cp.Object,exp));
            service.Object.Create(cp.Object, exp);
            service.Setup(x => x.Any(exp.Id));
            service.Object.Any(exp.Id);
            service.Verify(i => i.Create(cp.Object, exp));
            service.Verify(i => i.Any(exp.Id));
        }
        [Fact]
        public void DeleteTest()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<TaskItem>>();
            var mockContext = new Mock<ApplicationDbContext>();

            mockContext.Setup(item => item.Tasks).Returns(mockDbSet.Object);

            var repository = new Mock<TaskRepository>(mockContext.Object);

            var userRep = new Mock<UserRepository>(mockContext.Object);

            var mapper = new Mock<IMapper>();
            var task = new TaskItemDTO { Id = "1", Description = "Description", UserId = "1" };
            var taskItem = new TaskItem { Id = "1", Description = "Description", UserId = "1" };
            mapper.Setup(x => x.Map<TaskItem>(task)).Returns(taskItem);
            mapper.Setup(x => x.Map<TaskItemDTO>(taskItem)).Returns(task);

            var userService = new Mock<UserService>(userRep.Object);

            var service = new Mock<TaskService>(repository.Object, userService.Object, mapper.Object);

            service.Setup(x => x.Delete(task.Id));
            service.Object.Delete(task.Id);

            service.Verify(i => i.Delete(task.Id));

        }

        private IEnumerable<TaskItemDTO> GetTestListDTO()
        {
            return new[]
            {
                new TaskItemDTO{Id="1",Description="aaaa"},
                new TaskItemDTO{Id="2",Description="bbbb"},
                new TaskItemDTO{Id="3",Description="cccc"}
            };
        }
        private IEnumerable<TaskItem> GetTestList()
        {
            return new[]
            {
                new TaskItem{Id="1",Description="aaaa"},
                new TaskItem{Id="2",Description="bbbb"},
                new TaskItem{Id="3",Description="cccc"}
            };
        }
    }
}
