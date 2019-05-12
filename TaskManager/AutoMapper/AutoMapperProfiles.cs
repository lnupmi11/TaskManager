using AutoMapper;
using TaskManager.DAL.Models;
using TaskManager.DTO.Task;
using TaskManager.DTO.Models.UserManagement;

namespace TaskManager.AutoMapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
           CreateMap<TaskItem, TaskItemDTO>().ReverseMap();
           CreateMap<UserProfile, UserProfileDTO>().ReverseMap();
        }
    }
}
