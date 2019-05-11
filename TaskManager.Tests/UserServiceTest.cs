using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private UserService svc;
        private IEnumerable<UserProfile> list;
        public UserServiceTest()
        {
            svc = SetUpService();
            list = GetTestCollection();
        }
        [Fact]
        public void GetUserProfilesTest()
        {
            // Act
            IEnumerable<UserProfile> actual = svc.GetUserProfiles();

            // Assert
            Assert.Equal(actual.Count(), list.Count());
        }
        [Fact]
        public void GetUserProfileTest()
        {
            // Arrange
            var claims = new Mock<ClaimsPrincipal>();
            var identity = new Mock<ClaimsIdentity>();
            identity.Setup(i => i.IsAuthenticated)
                .Returns(true);
            claims.Setup(i => i.Identity)
                .Returns(identity.Object);
            claims.Setup(i => i.FindFirst(It.IsAny<string>()))
                .Returns(new Claim(ClaimTypes.NameIdentifier, "1"));
            var list1 = GetTestCollection().AsQueryable();
            var mockSet = new Mock<DbSet<UserProfile>>();
            mockSet.As<IQueryable<UserProfile>>().Setup(p => p.Provider).Returns(list1.Provider);
            mockSet.As<IQueryable<UserProfile>>().Setup(p => p.Expression).Returns(list1.Expression);
            mockSet.As<IQueryable<UserProfile>>().Setup(p => p.ElementType).Returns(list1.ElementType);
            mockSet.As<IQueryable<UserProfile>>().Setup(p => p.GetEnumerator()).Returns(list1.GetEnumerator);

            var mockCtx = new Mock<ApplicationDbContext>();
            mockCtx.Setup(p => p.UserProfiles).Returns(mockSet.Object);

            var repo = new Mock<IRepository<UserProfile>>();
            repo.Setup(r => r.Find(It.IsAny<string>()))
                .Returns(list.First(i=>i.Id=="1"));
            var svc1 = new UserService(repo.Object);

            // Act
            var user = svc1.GetUserProfile(claims.Object);

            // Assert
            Assert.Equal("aaa", user.FirstName);
        }
        [Fact]
        public void GetUserProfileTestByIds()
        {
            // Arrange
            var claims = new Mock<ClaimsPrincipal>();
            var identity = new Mock<ClaimsIdentity>();
            identity.Setup(i => i.IsAuthenticated)
                .Returns(true);
            claims.Setup(i => i.Identity)
                .Returns(identity.Object);
            claims.Setup(i => i.FindFirst(It.IsAny<string>()))
                .Returns(new Claim(ClaimTypes.NameIdentifier, "1"));
            var list1 = GetTestCollection().AsQueryable();
            var mockSet = new Mock<DbSet<UserProfile>>();
            mockSet.As<IQueryable<UserProfile>>().Setup(p => p.Provider).Returns(list1.Provider);
            mockSet.As<IQueryable<UserProfile>>().Setup(p => p.Expression).Returns(list1.Expression);
            mockSet.As<IQueryable<UserProfile>>().Setup(p => p.ElementType).Returns(list1.ElementType);
            mockSet.As<IQueryable<UserProfile>>().Setup(p => p.GetEnumerator()).Returns(list1.GetEnumerator);

            var mockCtx = new Mock<ApplicationDbContext>();
            mockCtx.Setup(p => p.UserProfiles).Returns(mockSet.Object);

            var repo = new Mock<IRepository<UserProfile>>();
            repo.Setup(r => r.GetAllByIds(It.IsAny<List<string>>()))
                .Returns(new List<UserProfile> { list.First(i => i.Id == "1") });
            var svc1 = new UserService(repo.Object);

            // Act
            var user = svc1.GetUserProfilesByIds(new List<string>{"1"});

            // Assert
            Assert.Equal("aaa", user.First().FirstName);
        }
        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public void GetUserProfileByIdTest(string id)
        {
            // Arrange
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

            var service = new UserService(repository.Object);


            // Act
            service.Update(expected);

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
            var service = new UserService(repository.Object);

            // Act
            service.Delete(expected);

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
            var data = GetTestCollection().AsQueryable();
            var mockSet = new Mock<DbSet<UserProfile>>();
            mockSet.As<IQueryable<UserProfile>>().Setup(p => p.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<UserProfile>>().Setup(p => p.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<UserProfile>>().Setup(p => p.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<UserProfile>>().Setup(p => p.GetEnumerator()).Returns(data.GetEnumerator);

            var mockCtx = new Mock<ApplicationDbContext>();
            mockCtx.Setup(p => p.UserProfiles).Returns(mockSet.Object);

            var repository = new UserRepository(mockCtx.Object);

            var service = new UserService(repository);
            return service;
        }
    }
}
