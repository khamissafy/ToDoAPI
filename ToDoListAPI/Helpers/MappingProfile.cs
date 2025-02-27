using AutoMapper;
using ToDoListAPI.Models.ToDoListManagement.DB_Models;
using ToDoListAPI.Models.ToDoListManagement.DTOs;
using ToDoListAPI.Models.UserManagement.Custom_Models;
using ToDoListAPI.Models.UserManagement.DB_Models;
using ToDoListAPI.Models.UserManagement.DTOs;

namespace ToDoListAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, AuthModel>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.isActive, opt => opt.MapFrom(src => src.isActive))
                .ForMember(dest => dest.isEmailAuthonticated, opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(dest => dest.subscriptionStartDate, opt => opt.MapFrom(src => src.subscriptionStartDate))
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id));

            CreateMap<TodoItem, TodoItemDto>().ReverseMap();
            CreateMap<CreateTodoDto, TodoItem>();
            CreateMap<UpdateTodoDto, TodoItem>();
            CreateMap<ActivityLogDTO, ActivityLog>();

        }
    }
}
