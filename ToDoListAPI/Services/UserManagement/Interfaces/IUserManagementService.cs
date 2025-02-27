using ToDoListAPI.Helpers;
using ToDoListAPI.Models.UserManagement.Custom_Models;
using ToDoListAPI.Models.UserManagement.DTOs;

namespace ToDoListAPI.Services.UserManagement.Interfaces
{
    public interface IUserManagementService
    {
        Task<AuthModel> GetTokenAsync(TokenRequestModel model, UserBasicInfo userBasicInfo);
        Task<(PaginatedList<ActivityLogDTO>? logs, int totalCount, string error)> GetAllActivityLogsAsync(ListLogsDTO query, string lang);
    }
}
