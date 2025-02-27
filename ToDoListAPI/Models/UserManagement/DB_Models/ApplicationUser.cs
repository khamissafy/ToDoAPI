using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Security;

namespace ToDoListAPI.Models.UserManagement.DB_Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(50)]
        public string FullName { get; set; }
        public DateTime subscriptionStartDate { get; set; } = DateTime.UtcNow;
        public DateTime? lastLogin { get; set; } = DateTime.UtcNow;
        public bool isActive { get; set; }
        public bool isDeleted { get; set; }
        [JsonIgnore]
        public List<RefreshToken>? RefreshTokens { get; set; }
        public int FailedLoginAttempts { get; set; }
    }

}
