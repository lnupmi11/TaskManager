using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.BLL.Services;
using TaskManager.DAL.EF;
using TaskManager.DAL.Interfaces;
using TaskManager.DAL.Models;
using TaskManager.DAL.Repositories;
using Xunit;

namespace TaskManager.Tests
{
    public class UserServiceTest
    {
        [Fact]
        public void GetUserProfilesTest()
        {
            // Arrange
            var list = GetTestCollection();
            var svc = SetUpService();

            // Act
            IEnumerable<UserProfile> actual = svc.GetUserProfiles();

            // Assert
            Assert.Equal(actual.Count(), list.Count());
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public void GetUserProfileByIdTest(string id)
        {
            // Arrange
            var svc = SetUpService();
            var dtos = GetTestCollection();
            var expected = dtos.First(i => i.Id == id);

            // Act
            var actual = svc.GetUserProfile(id);

            // Assert
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.FirstName, actual.FirstName);
            Assert.Equal(expected.LastName, actual.LastName);
        }

        [Fact]
        public void GetUserProfileByIdNotExistingTest()
        {
            // Arrange
            var svc = SetUpService();

            // Act
            var actual = svc.GetUserProfile("15");

            // Assert
            Assert.Null(actual);
        }


        [Fact]
        public void UpdateTest()
        {
            // Arrange
            var expected = new UserProfile
            {
                FirstName = "aaa",
                LastName = "aaa"
            };
            var repository = new Mock<IRepository<UserProfile>>();
            repository.Setup(r => r.GetAllByIds(new List<string>{expected.Id})).Returns(new List<UserProfile>{new UserProfile
            {
                FirstName = "aaa",
                LastName = "aaa"
                }});
            var svc = new UserService(repository.Object);


            // Act
            svc.Update(expected);

            // Assert
            repository.Verify(r => r.Update(It.IsAny<UserProfile>()), Times.Once());

        }

        [Fact]
        public void DeleteTest()
        {
            // Arrange
            var expected = new UserProfile()
            {
                Id = "1",
                FirstName = "aaa",
                LastName = "aaa"
            };
            var repository = new Mock<IRepository<UserProfile>>();
            repository.Setup(r => r.GetAllByIds(new List<string> { expected.Id })).Returns(new List<UserProfile>{new UserProfile
            {
                     Id = "1",
                FirstName = "aaa",
                LastName = "aaa"
                }});
            var svc = new UserService(repository.Object);

            // Act
            svc.Delete(expected);

            // Assert
            repository.Verify(r => r.Delete(It.IsAny<UserProfile>()), Times.Once());
          
        }

        private IEnumerable<UserProfile> GetTestCollection()
        {
            return new[]
            {
                new UserProfile
                {
                    Id="1",
                    FirstName="aaa",
                    LastName="aaa"
                },
                new UserProfile
                {
                    Id="2",
                    FirstName="aaa",
                    LastName="aaa"
                },
                new UserProfile
                {
                    Id="3",
                    FirstName="aaa",
                    LastName="aaa"
                }
            };
        }

        private UserService SetUpService()
        {
            var list = GetTestCollection().AsQueryable();
            var mockSet = new Mock<DbSet<UserProfile>>();
            mockSet.As<IQueryable<UserProfile>>().Setup(p => p.Provider).Returns(list.Provider);
            mockSet.As<IQueryable<UserProfile>>().Setup(p => p.Expression).Returns(list.Expression);
            mockSet.As<IQueryable<UserProfile>>().Setup(p => p.ElementType).Returns(list.ElementType);
            mockSet.As<IQueryable<UserProfile>>().Setup(p => p.GetEnumerator()).Returns(list.GetEnumerator);

            var mockCtx = new Mock<ApplicationDbContext>();
            mockCtx.Setup(p => p.UserProfiles).Returns(mockSet.Object);

            var repository = new UserRepository(mockCtx.Object);

            var svc = new UserService(repository);
            return svc;
        }
    }
}
