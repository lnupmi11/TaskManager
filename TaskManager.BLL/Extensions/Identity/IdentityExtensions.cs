using System.Security.Claims;

namespace TaskManager.BLL.Extensions.Identity
{
    public static class IdentityExtensions
    {
        public static IdentityExtension Implementation = new IdentityExtension();

        public static string GetUserId(this ClaimsPrincipal user){
            return Implementation.GetUserId(user);
        }
        public static string GetUserName(this ClaimsPrincipal user){
            return Implementation.GetUserName(user);
        }
        public static string GetUserRole(this ClaimsPrincipal user){
            return Implementation.GetUserRole(user);
        }
    }
}
