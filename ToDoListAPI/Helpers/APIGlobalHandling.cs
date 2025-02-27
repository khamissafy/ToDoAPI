using Azure.Core;
using System.Security.Claims;

namespace ToDoListAPI.Helpers
{
    public class APIResponse
    {
        public string ErrorMessage { get; set; } = "";
        public int Count { get; set; } = 0;
        public object? Data { get; set; } = null;
    }

    public class UserBasicInfo
    {
        public string? UserId { get; set; }
        public string? IpAddress { get; set; }
        public string Lang { get; set; }
    }

    public static class UserBasicInfoHandler
    {
        public static UserBasicInfo GetUserBasicInfo(HttpContext context, ClaimsPrincipal? user)
        {
            var ipAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                          ?? context.Request.Headers["X-Real-IP"].FirstOrDefault()
                          ?? context.Connection.RemoteIpAddress?.ToString();
            ipAddress = ipAddress?.Split(' ').LastOrDefault();

            var userId = user?.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;

            return new UserBasicInfo
            {
                IpAddress = ipAddress,
                UserId = userId,
                Lang = ErrorHandling.GetLanguage(context.Request)
            };
        }
    }

    public static class ErrorHandling
    {
        public static string GetLanguage(HttpRequest request)
        {
            if (request.Headers.TryGetValue("lang", out var lang))
            {
                return lang.ToString().ToLower() == "ar" ? "ar" : "en";
            }
            return "en"; // Default to English
        }
    }


}
