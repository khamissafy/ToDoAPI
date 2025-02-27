using ToDoListAPI.Models.ToDoListManagement.DB_Models;
using ToDoListAPI.Models.ToDoListManagement.DTOs;

namespace ToDoListAPI.Services.ToDoList.Interfaces
{
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoItem>> GetAllAsync(ListTodoDTO dto);
        Task<TodoItem?> GetByIdAsync(Guid id);
        Task<int> GetCountAsync(ListTodoDTO dto);
        Task AddAsync(TodoItem todo);
        Task UpdateAsync(TodoItem todo);
    }
}
