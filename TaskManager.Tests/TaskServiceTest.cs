using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.BLL.Extensions.Identity;
using TaskManager.BLL.Services;
using TaskManager.DAL.EF;
using TaskManager.DAL.Models;
using TaskManager.DAL.Models.Enums;
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
            var list = GetTestList().AsQueryable();
            var service = SetUpService();

            // Act
            var actual = service.GetAll();

            // Assert
            Assert.Equal(list.Count(), actual.Count());
        }
        [Fact]
        public void GetActiveListTest()
        {
            var list = GetTestList().AsQueryable();
            var service = SetUpService();

            // Act
            var actual = service.GetActiveByFilters(priorities:new List<Priority>{Priority.Critical},category:Category.Work);

            // Assert
            Assert.Equal(2, actual.Count());
        }
        [Fact]
        public void GetArchivedListTest()
        {
            var list = GetTestList().AsQueryable();
            var service = SetUpService();

            // Act
            var actual = service.GetArchivedByFilters(priorities: new List<Priority> { Priority.Critical }, category: Category.Work);

            // Assert
            Assert.Equal(1, actual.Count());
        }
        [Fact]
        public void GetUsersActiveListTest()
        {
            var list = GetTestList().AsQueryable();
            var service = SetUpService();
            var claims = new Mock<ClaimsPrincipal>();
            var identity = new Mock<ClaimsIdentity>();
            identity.Setup(i => i.IsAuthenticated)
                .Returns(true);
            claims.Setup(i => i.Identity)
                .Returns(identity.Object);
            claims.Setup(i => i.FindFirst(It.IsAny<string>()))
                .Returns(new Claim(ClaimTypes.NameIdentifier, "1"));
            // Act
            var actual = service.GetUserActiveTasksByFilters(claims.Object,priorities: new List<Priority> { Priority.Critical }, category: Category.Work);

            // Assert
            Assert.Equal(2, actual.Count());
        }
        [Fact]
        public void GetUsersArchivedListTest()
        {
            var list = GetTestList().AsQueryable();
            var service = SetUpService();
            var claims = new Mock<ClaimsPrincipal>();
            var identity = new Mock<ClaimsIdentity>();
            identity.Setup(i => i.IsAuthenticated)
                .Returns(true);
            claims.Setup(i => i.Identity)
                .Returns(identity.Object);
            claims.Setup(i => i.FindFirst(It.IsAny<string>()))
                .Returns(new Claim(ClaimTypes.NameIdentifier, "1"));
            // Act
            var actual = service.GetUserArchivedTasksByFilters(claims.Object, priorities: new List<Priority> { Priority.Critical }, category:null);
            // Assert
            Assert.NotNull(actual);
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

            var service = new Mock<TaskService>(repository.Object, userRep.Object, mapper.Object);

            var principal = new Mock<ClaimsPrincipal>();
            principal.Setup(b => b.Identity.IsAuthenticated).Returns(true);
            principal.Setup(b => b.FindFirst(It.IsAny<string>())).Returns(new Claim(ClaimTypes.NameIdentifier, "1"));

            service.Setup(x => x.Create(principal.Object, expTaskDTO));
            service.Object.Create(principal.Object, expTaskDTO);
            service.Verify(i => i.Create(principal.Object, expTaskDTO));
        }
        [Fact]
        public void UpdateTest()
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

            repository.Setup(i => i.FindAsNoTracking(It.IsAny<string>())).Returns(taskItem);

            var userService = new Mock<UserService>(userRep.Object);

            var service = new Mock<TaskService>(repository.Object, userRep.Object, mapper.Object);
            var cp = new Mock<ClaimsPrincipal>();
            cp.Setup(m => m.HasClaim(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var mockMyImplementation = new Mock<IdentityExtension>();

            IdentityExtensions.Implementation = mockMyImplementation.Object;
            mockMyImplementation.Setup(m => m.GetUserId(cp.Object)).Returns("1");
            // Act 
            service.Setup(i=>i.Create(cp.Object, task));
            service.Object.Create(cp.Object, task);
            service.Setup(i=>i.Update(task));
            service.Object.Update(task);
            // Assert
            service.Verify(i => i.Update(task));
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

            var service = new Mock<TaskService>(repository.Object, userRep.Object, mapper.Object);
            var cp = new Mock<ClaimsPrincipal>();
            cp.Setup(m => m.HasClaim(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var mockMyImplementation = new Mock<IdentityExtension>();

            IdentityExtensions.Implementation = mockMyImplementation.Object;
            mockMyImplementation.Setup(m => m.GetUserId(cp.Object)).Returns("1");
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

            var service = new Mock<TaskService>(repository.Object, userRep.Object, mapper.Object);

            service.Setup(x => x.Delete(task.Id));
            service.Object.Delete(task.Id);

            service.Verify(i => i.Delete(task.Id));

        }

        public IEnumerable<TaskItemDTO> GetTestListDTO()
        {
            return new[]
            {
                new TaskItemDTO{Id="1",Description="aaaa"},
                new TaskItemDTO{Id="2",Description="bbbb"},
                new TaskItemDTO{Id="3",Description="cccc"}
            };
        }
        public IEnumerable<TaskItem> GetTestList()
        {
            return new[]
            {
                new TaskItem{Id="1",Description="aaaa",Status=Status.Active ,Priority=Priority.Critical,Category=Category.Work,UserId="1"},
                new TaskItem{Id="2",Description="bbbb",Status=Status.Active ,Priority=Priority.Critical,Category=Category.Work,UserId="1"},
                new TaskItem{Id="3",Description="cccc",Status = Status.Closed, Priority=Priority.Critical,Category=Category.Work,UserId="1"}
            };
        }
        private TaskService SetUpService()
        {
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
            var task = new TaskItemDTO { Id = "1", Description = "aaaa",Status=Status.Active ,Priority=Priority.Critical,Category=Category.Work, UserId = "1" };
            var taskItem = new TaskItem { Id = "1", Description = "aaaa", Status = Status.Active, Priority = Priority.Critical, Category = Category.Work, UserId = "1" };
            mapper.Setup(x => x.Map<TaskItemDTO>(taskItem)).Returns(task);
            var task1 = new TaskItemDTO { Id = "1", Description = "bbbb", Status = Status.Active, Priority = Priority.Critical, Category = Category.Work, UserId = "1" };
            var taskItem1 = new TaskItem { Id = "1", Description = "bbbb", Status = Status.Active, Priority = Priority.Critical, Category = Category.Work, UserId = "1" };
            mapper.Setup(x => x.Map<TaskItemDTO>(taskItem1)).Returns(task1);
            var task2 = new TaskItemDTO { Id = "1", Description = "cccc" , Status = Status.Closed, Priority = Priority.Critical, Category = Category.Work, UserId = "1" };
            var taskItem2 = new TaskItem { Id = "1", Description = "cccc", Status = Status.Closed, Priority = Priority.Critical, Category = Category.Work, UserId = "1" };
            mapper.Setup(x => x.Map<TaskItemDTO>(taskItem2)).Returns(task2);

            var userService = new Mock<UserService>(userRep.Object);

            var service = new TaskService(repository, userRep.Object, mapper.Object);
            return service;

        }
    }
}
