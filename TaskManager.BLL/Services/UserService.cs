using System.Collections.Generic;
using System.Security.Claims;
using TaskManager.DAL.Models;
using TaskManager.BLL.Extensions.Identity;
using TaskManager.DAL.Interfaces;
using TaskManager.BLL.Interfaces;

namespace TaskManager.BLL.Services
{
    public class UserService : IUserService
    {
        private IRepository<UserProfile> _userRepository;

        public UserService(IRepository<UserProfile> userRepository)
        {
            _userRepository = userRepository;
        }

        public UserProfile GetUserProfile(ClaimsPrincipal principal)
        {
            return _userRepository.Find(principal.GetUserId());
        }

        public UserProfile GetUserProfile(string id)
        {
            return _userRepository.Find(id);
        }

        public string GetUserProfileId(ClaimsPrincipal principal)
        {
            return principal.GetUserId();
        }

        public IEnumerable<UserProfile> GetUserProfiles()
        {
            return _userRepository.GetAll();
        }

        public void ChangeFirstName(UserProfile user, string firstName)
        {
            user.FirstName = firstName;
            _userRepository.Update(user);
        }

        public void ChangeFirstName(ClaimsPrincipal principal, string firstName)
        {
            var user = GetUserProfile(principal);
            ChangeFirstName(user, firstName);
        }

        public void ChangeSecondName(UserProfile user, string lastName)
        {
            user.LastName = lastName;
            _userRepository.Update(user);
        }

        public void ChangeSecondName(ClaimsPrincipal principal, string secondName)
        {
            var user = GetUserProfile(principal);
            ChangeSecondName(user, secondName);
        }

        public void Update(UserProfile user)
        {
            _userRepository.Update(user);
        }

        public void Delete(UserProfile user)
        {
            _userRepository.Delete(user);
        }
    }
}
