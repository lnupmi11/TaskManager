using System.Collections.Generic;
using System.Security.Claims;
using TaskManager.DAL.Models;
using TaskManager.DAL.Models.Enums;

namespace TaskManager.BLL.Interfaces
{
    public interface IUserService
    {
        UserProfile GetUserProfile(ClaimsPrincipal principal);
        UserProfile GetUserProfile(string id);
        string GetUserProfileId(ClaimsPrincipal principal);
        IEnumerable<UserProfile> GetUserProfiles();
        IEnumerable<UserProfile> GetUserProfilesByIds(IEnumerable<string> ids);
        void ChangeFirstName(UserProfile user, string firstName);
        void ChangeFirstName(ClaimsPrincipal principal, string firstName);
        void ChangeSecondName(UserProfile user, string lastName);
        void ChangeSecondName(ClaimsPrincipal principal, string secondName);
        void Update(UserProfile user);
        void Delete(UserProfile user);
    }
}
