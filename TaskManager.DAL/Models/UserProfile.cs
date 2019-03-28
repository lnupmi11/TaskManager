using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TaskManager.DAL.Models
{
    // Add UserProfile data for application users by adding properties to the ApplicationUser class
    public class UserProfile : IdentityUser
    {
        public UserProfile()
        {
            Tasks = new List<TaskItem>();
        }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public DateTime RegistredOn { get; set; }

        public virtual ICollection<TaskItem> Tasks { get; set; }
    }
}
