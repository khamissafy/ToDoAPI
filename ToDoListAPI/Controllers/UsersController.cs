using Microsoft.AspNetCore.Mvc;
using ToDoListAPI.Helpers;
using ToDoListAPI.Models.UserManagement.DTOs;
using ToDoListAPI.Services.UserManagement.Interfaces;

namespace ToDoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserManagementService _authService;

        public UsersController(IUserManagementService _authService)
        {
            this._authService = _authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)
        {
            var userInfo = UserBasicInfoHandler.GetUserBasicInfo(HttpContext, User);

            var result = await _authService.GetTokenAsync(model, userInfo);

            if (!result.IsAuthenticated)
            {
                return StatusCode(403, 
                    new APIResponse { ErrorMessage = result.ErrorMessage });
            }

            return Ok(new APIResponse { Data = result, Count = 1 });
        }

        [HttpGet("logs")]
        public async Task<IActionResult> GetAllUsersLogs([FromQuery] ListLogsDTO query)
        {
            string lang = ErrorHandling.GetLanguage(Request);

            var (logs, totalCount, error) = await _authService.GetAllActivityLogsAsync(query, lang);
            if (!string.IsNullOrEmpty(error))
                return BadRequest(new APIResponse { ErrorMessage =  error });

            return Ok(new APIResponse { Data = logs, Count = totalCount });
        }

    }
}
