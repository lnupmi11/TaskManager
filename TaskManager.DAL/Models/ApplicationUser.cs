using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TaskManager.DAL.Models
{
    // Add UserProfile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Tasks = new List<TaskItem>();
        }

        public virtual ICollection<TaskItem> Tasks { get; set; }
    }
}
