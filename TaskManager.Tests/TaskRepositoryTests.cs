using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.DAL.EF;
using TaskManager.DAL.Models;
using TaskManager.DAL.Repositories;
using Xunit;

namespace TaskManager.Tests
{
    public class TaskRepositoryTests
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

            // Act
            var actual = repository.GetAll();

            Assert.Equal(list.Count(), actual.Count());
        }

        [Fact]
        public void GetByIdTest()
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

            repository.Create(list.First());
            // Act
            var actual = repository.GetAllWhere(i => i.Id == list.First().Id);

            //// Assert
            Assert.Equal(list.First().Id, actual.First().Id);
        }

        [Theory]
        [InlineData("4")]
        [InlineData("5")]
        [InlineData("6")]
        public void GetByNotExistingIdTest(string id)
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
            // Act
            var actual = repository.GetAllWhere(i => i.Id == id);

            // Assert
            Assert.True(!actual.Any());
        }



        [Fact]
        public void AddTest()
        {
            // Arrange
            var mockDbSet = new Mock<DbSet<TaskItem>>();
            var mockContext = new Mock<ApplicationDbContext>();

            mockContext.Setup(item => item.Tasks).Returns(mockDbSet.Object);

            var repository = new TaskRepository(mockContext.Object);

            // Act 
            var exp = new TaskItem { Id = "4", Description = "ssss" };
            repository.Create(exp);

            // Assert
            mockDbSet.Verify(m => m.Add(It.IsAny<TaskItem>()), Times.Once());
        }


        [Fact]
        public void DeleteTest()
        {
            var task = new TaskItem { Id = "3", Description = "ssss" };

            // Arrange
            var mockDbSet = new Mock<DbSet<TaskItem>>();
            var mockContext = new Mock<ApplicationDbContext>();

            mockContext.Setup(item => item.Tasks).Returns(mockDbSet.Object);

            var mockpersonRepository = new Mock<TaskRepository>(mockContext.Object);

            mockpersonRepository.Setup(x => x.Delete(task.Id));
            mockpersonRepository.Object.Delete(task.Id);

            mockpersonRepository.Verify(x => x.Delete(task.Id), Times.Once);

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
