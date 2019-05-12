using System;
using System.Collections.Generic;
using System.Security.Claims;
using TaskManager.DAL.Models;
using TaskManager.BLL.Extensions.Identity;
using TaskManager.DAL.Interfaces;
using TaskManager.BLL.Interfaces;
using AutoMapper;
using TaskManager.DTO.Models.UserManagement;
using TaskManager.DAL.Models.Enums;
using System.Linq;

namespace TaskManager.BLL.Services
{
    public class UserService : IUserService
    {
        private IRepository<UserProfile> _userRepository;
        private readonly IMapper _mapper;

        private const int BAN_END_YEAR = 3000;
        private const int BAN_END_YEAR_PAST = 2000;

        private const int BAN_END_MONTH = 1;
        private const int BAN_END_DAY = 1;

        private DateTime _lockoutEndDate = new DateTime(BAN_END_YEAR, BAN_END_MONTH, BAN_END_DAY);
        private DateTime _lockoutEndDatePast = new DateTime(BAN_END_YEAR_PAST, BAN_END_MONTH, BAN_END_DAY);


        public UserService(IRepository<UserProfile> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public virtual UserProfile GetUserProfile(ClaimsPrincipal principal)
        {
            return _userRepository.Find(principal.GetUserId());
        }

        public virtual UserProfile GetUserProfile(string id)
        {
            return _userRepository.Find(id);
        }

        public virtual string GetUserProfileId(ClaimsPrincipal principal)
        {
            return principal.GetUserId();
        }

        public virtual IEnumerable<UserProfile> GetUserProfiles()
        {
            return _userRepository.GetAll();
        }

        public IEnumerable<UserProfile> GetUserProfilesByIds(IEnumerable<string> ids)
        {
            return _userRepository.GetAllByIds(ids);
        }

        public virtual void ChangeFirstName(UserProfile user, string firstName)
        {
            user.FirstName = firstName;
            _userRepository.Update(user);
        }

        public virtual void ChangeFirstName(ClaimsPrincipal principal, string firstName)
        {
            var user = GetUserProfile(principal);
            ChangeFirstName(user, firstName);
        }

        public virtual void ChangeSecondName(UserProfile user, string lastName)
        {
            user.LastName = lastName;
            _userRepository.Update(user);
        }

        public virtual void ChangeSecondName(ClaimsPrincipal principal, string secondName)
        {
            var user = GetUserProfile(principal);
            ChangeSecondName(user, secondName);
        }

        public virtual void Update(UserProfile user)
        {
            _userRepository.Update(user);
        }

        public virtual void Delete(UserProfile user)
        {
            _userRepository.Delete(user);
        }

        public virtual bool IsAccountLocked(UserProfile user)
        {
            return user.LockoutEnabled && user.LockoutEnd != null && user.LockoutEnd > DateTime.Now;
        }

        public virtual void LockAccount(UserProfile user)
        {
            user.LockoutEnabled = true;
            user.LockoutEnd = _lockoutEndDate;
            Update(user);
        }

        public virtual void UnlockAccount(UserProfile user)
        {
            user.LockoutEnabled = false;
            user.LockoutEnd = _lockoutEndDatePast;
            Update(user);
        }

        public virtual int CountInactiveTasks(UserProfile user)
        {
            int countInactive = user.Tasks.Count(p => (p.Status == Status.ToDo && p.EndDate < DateTime.Today));

            return countInactive;
        }

        public virtual IEnumerable<UserProfileDTO> GetUsers(IEnumerable<string> ids)
        {
            var users = GetUserProfilesByIds(ids);
            List<UserProfileDTO> usersDTO = new List<UserProfileDTO>();

            foreach (var user in users)
            {
                var userDTO = _mapper.Map<UserProfileDTO>(user);
                userDTO.InactiveTasksCount = CountInactiveTasks(user);
                userDTO.IsAccountLocked = IsAccountLocked(user);
                usersDTO.Add(userDTO);
            }

            return usersDTO;
        }
    }
}
