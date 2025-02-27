namespace ToDoListAPI.Models.UserManagement.DTOs
{
    public class ActivityLogDTO
    {
        public string ActionPerformed { get; set; }
        public string? IpAddress { get; set; }
        public DateTime PerformedAt { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; }
        public string UserName { get; set; }

    }
}
