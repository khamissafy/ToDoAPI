using ToDoListAPI.Helpers;

namespace ToDoListAPI.Models.UserManagement.DTOs
{
    public class ListLogsDTO : ListObjectsBase
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; internal set; }
        public string? UserId { get; set; }
    }
}
