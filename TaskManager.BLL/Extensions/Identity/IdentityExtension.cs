using System;
using System.Security.Claims;
using TaskManager.DAL.Models.Enums;

namespace TaskManager.BLL.Extensions.Identity
{
    public class IdentityExtension : IIdentityExtension
    {
        public virtual string GetUserId(ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return null;
            }

            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public virtual string GetUserName(ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return null;
            }

            return user.Identity.Name;
        }

        public virtual string GetUserRole(ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
            {
                return null;
            }

            string role = "";

            if (user.IsInRole(Enum.GetName(typeof(Roles), Roles.Admin)))
            {
                role = "Admin";
            }
            else if (user.IsInRole(Enum.GetName(typeof(Roles), Roles.User)))
            {
                role = "User";
            }

            return role;
        }
    }
}
