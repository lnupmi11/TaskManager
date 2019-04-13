using AutoMapper;
using TaskManager.DAL.Models;
using TaskManager.DTO.Task;

namespace TaskManager.AutoMapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
           CreateMap<TaskItem, TaskItemDTO>().ReverseMap();
        }
    }
}
