using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.BLL.Services;
using TaskManager.DAL.EF;
using TaskManager.DAL.Models;
using TaskManager.DAL.Repositories;
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

            var service = new TaskService(repository);

            // Act
            var actual = service.GetAll();

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
            var expTask = new TaskItem { Id = "4", Description = "ssss" };
            var service = new Mock<TaskService>(repository.Object);
            service.Setup(x => x.Create(expTask));
            service.Object.Create(expTask);
            service.Verify(i => i.Create(expTask));
        }
        [Fact]
        public void UpdateTest()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<TaskItem>>();
            var mockContext = new Mock<ApplicationDbContext>();

            mockContext.Setup(item => item.Tasks).Returns(mockDbSet.Object);

            var repository = new TaskRepository(mockContext.Object);

            // Act 
            var exp = new TaskItem { Id = "4", Description = "ssss" };
            var service = new TaskService(repository);
            service.Create(exp);
            var exp2 = new TaskItem { Id = "4", Description = "ddddd" };
            service.Update(exp2);
            // Assert
            mockDbSet.Verify(m => m.Update(It.IsAny<TaskItem>()), Times.Once());
        }
        [Fact]
        public void FindTest()
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

            var service = new TaskService(repository);

            // Act 
            var res=service.Find(list.First().Id);

            // Assert
           

            Assert.Equal(res.Id, list.First().Id);
        }

        [Fact]
        public void AnyTest()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<TaskItem>>();
            var mockContext = new Mock<ApplicationDbContext>();

            mockContext.Setup(item => item.Tasks).Returns(mockDbSet.Object);

            var repository = new TaskRepository(mockContext.Object);

            // Act 
            var exp = new TaskItem { Id = "4", Description = "ssss" };
            var service = new Mock<TaskService>(repository);
            service.Setup(x => x.Create(exp));
            service.Object.Create(exp);
            service.Setup(x => x.Any(exp.Id));
            service.Object.Any(exp.Id);
            service.Verify(i => i.Create(exp));
            service.Verify(i => i.Any(exp.Id));
        }
        [Fact]
        public void DeleteTest()
        {
            var task = new TaskItem { Id = "3", Description = "ssss" };

            // Arrange
            var mockDbSet = new Mock<DbSet<TaskItem>>();
            var mockContext = new Mock<ApplicationDbContext>();

            mockContext.Setup(item => item.Tasks).Returns(mockDbSet.Object);

            var repository = new TaskRepository(mockContext.Object);

            var service = new Mock<TaskService>(repository);

            service.Setup(x => x.Delete(task));
            service.Object.Delete(task);

            service.Verify(i => i.Delete(task));

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
