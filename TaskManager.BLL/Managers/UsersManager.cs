using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.DAL.Models;
using TaskManager.DAL.Repositories;
using TaskManager.BLL.Extensions.Identity;

namespace TaskManager.BLL.Managers
{
    public class UsersManager : IDisposable
    {
        private readonly WorkContext _context;

        public UsersManager(WorkContext context)
        {
            _context = context;
        }

        public IEnumerable<UserProfile> GetUserProfiles()
        {
            return _context.Users.GetAll();
        }

        #region UserProfile entities management

        public async Task ChangeFirstNameAsync(UserProfile user, string firstName)
        {
            user.FirstName = firstName;
            _context.Users.Update(user);
            await _context.SaveAsync();
        }

        public async Task ChangeFirstNameAsync(ClaimsPrincipal principal, string firstName)
        {
            var user = GetUserProfile(principal);
            await ChangeFirstNameAsync(user, firstName);
        }

        public async Task ChangeSecondNameAsync(UserProfile user, string lastName)
        {
            user.LastName = lastName;
            _context.Users.Update(user);
            await _context.SaveAsync();
        }

        public async Task ChangeSecondNameAsync(ClaimsPrincipal principal, string secondName)
        {
            var user = GetUserProfile(principal);
            await ChangeSecondNameAsync(user, secondName);
        }

        public UserProfile GetUserProfile(ClaimsPrincipal principal)
        {
            UserProfile userProfile = _context.Users.Find(principal.GetUserId());
            return userProfile;
        }

        public UserProfile GetUserProfileById(string id)
        {
            UserProfile userProfile = _context.Users.Find(id);
            return userProfile;
        }

        public string GetUserProfileId(ClaimsPrincipal principal)
        {
            return principal.GetUserId();
        }

        public async Task UpdateSaveAsync(UserProfile user)
        {
            _context.Users.Update(user);
            await _context.SaveAsync();
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && _context != null)
                {
                    _context.Dispose(true);
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
