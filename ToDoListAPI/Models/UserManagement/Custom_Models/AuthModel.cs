using System.Security;

namespace ToDoListAPI.Models.UserManagement.Custom_Models
{
    public class AuthModel
    {
        public string id { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        //public DateTime ExpiresOn { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiration { get; set; }
        public bool isActive { get; set; }
        public bool isEmailAuthonticated { get; set; } = true;
        public string FullName { get; set; }
        public DateTime subscriptionStartDate { get; set; }
        public DateTime TokenExpiresOn { get; set; }
    }

}
