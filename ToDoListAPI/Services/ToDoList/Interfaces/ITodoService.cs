using ToDoListAPI.Helpers;
using ToDoListAPI.Models.ToDoListManagement.DTOs;

namespace ToDoListAPI.Services.ToDoList.Interfaces
{
    public interface ITodoService
    {
        Task<(PaginatedList<TodoItemDto>? todos, int totalCount, string error)> GetAllAsync(ListTodoDTO query);
        Task<(TodoItemDto? todo, string error)> GetByIdAsync(Guid id);
        Task<(bool success, string error)> AddAsync(CreateTodoDto todoDto, UserBasicInfo actionTakenBy);
        Task<(bool success, string error)> UpdateAsync(Guid id, UpdateTodoDto todoDto, UserBasicInfo actionTakenBy);
        Task<(bool success, string error)> SoftDeleteAsync(Guid id, UserBasicInfo actionTakenBy);
    }
}
