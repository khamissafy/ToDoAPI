using System.ComponentModel.DataAnnotations;

namespace ToDoListAPI.Models.UserManagement.DB_Models
{
    public class ActivityLog
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string ActionPerformed { get; set; }
        [Required]
        public string ActionPerformedAr { get; set; }

        public string? IpAddress { get; set; }
        public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string UserId { get; set; }
    }
}
