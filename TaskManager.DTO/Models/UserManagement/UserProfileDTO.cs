using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaskManager.DAL.Models;

namespace TaskManager.DTO.Models.UserManagement
{
    public class UserProfileDTO
    {
        public UserProfileDTO()
        {
            Tasks = new List<TaskItem>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime RegistredOn { get; set; }

        public virtual ICollection<TaskItem> Tasks { get; set; }

        public int InactiveTasksCount { get; set; }

        public string Id { get; set; }

        public bool IsAccountLocked { get; set; }

        public string Email { get; set; }
    }
}
