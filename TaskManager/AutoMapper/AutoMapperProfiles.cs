using AutoMapper;
using TaskManager.DAL.Models;
using TaskManager.DTO.Task;
using TaskManager.DTO.Models.UserManagement;
using TaskManager.DTO.Models.Category;

namespace TaskManager.AutoMapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<TaskItem, TaskItemDTO>();
            CreateMap<TaskItem, TaskItemDTOResponse>();
            CreateMap<TaskItemDTO, TaskItem>()
                .ForMember(task => task.Categories, opt => opt.Ignore());
            CreateMap<UserProfile, UserProfileDTO>().ReverseMap();
            CreateMap<CategoryItem, CategoryItemDTO>().ReverseMap();
        }
    }
}
