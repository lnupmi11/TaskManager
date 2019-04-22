using System.Security.Claims;

namespace TaskManager.BLL.Extensions.Identity
{
    public interface IIdentityExtension
    {
        string GetUserId(ClaimsPrincipal user);
        string GetUserName(ClaimsPrincipal user);
        string GetUserRole(ClaimsPrincipal user);
    }
}
