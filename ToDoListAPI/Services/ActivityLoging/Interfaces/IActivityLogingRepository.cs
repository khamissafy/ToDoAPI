using ToDoListAPI.Models.UserManagement.DB_Models;
using ToDoListAPI.Models.UserManagement.DTOs;

namespace ToDoListAPI.Services.ActivityLoging.Interfaces
{
    public interface IActivityLogingRepository
    {
        Task AddActivityLog(string? userId, string descriptionAr, string descriptionEn, string? ipAddress);
        Task<IEnumerable<ActivityLogDTO>> GetAllAsync(ListLogsDTO dto, string lang);
        Task<int> GetCountAsync(ListLogsDTO dto);
    }
}
