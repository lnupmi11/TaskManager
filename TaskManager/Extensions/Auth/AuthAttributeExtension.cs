using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;

namespace TaskManager.Extensions.Auth
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AuthAttributeExtension : AuthorizeAttribute
    {
        public AuthAttributeExtension(params object[] roles)
        {
            if (roles.Any(r => r.GetType().BaseType != typeof(Enum)))
            {
                throw new ArgumentException("Invalid role");
            }

            Roles = string.Join(",", roles.Select(r => Enum.GetName(r.GetType(), r)));
        }
    }
}
