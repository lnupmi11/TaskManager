using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime RegistredOn { get; set; }

        public virtual ICollection<TaskItem> Tasks { get; set; }
    }
}
