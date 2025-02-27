using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ToDoListAPI.Helpers;
using ToDoListAPI.Models.UserManagement.Custom_Models;
using ToDoListAPI.Models.UserManagement.DB_Models;
using ToDoListAPI.Models.UserManagement.DTOs;
using ToDoListAPI.Services.UserManagement.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ToDoListAPI.Services.ActivityLoging.Interfaces;
using AutoMapper;
using ToDoListAPI.Models.ToDoListManagement.DTOs;

namespace ToDoListAPI.Services.UserManagement.Classes
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IActivityLogingRepository _activityLogingRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWT _jwt;
        private readonly ILogger<UserManagementService> _logger;
        private readonly IMapper _mapper;

        public UserManagementService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt, IActivityLogingRepository activityLogingRepository, ILogger<UserManagementService> logger, IMapper mapper)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _activityLogingRepository = activityLogingRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<AuthModel> GetTokenAsync(TokenRequestModel model, UserBasicInfo userBasicInfo)
        {
            var authModel = new AuthModel();
            var retryAllowedCount = ReadConfiguration.ReadFromAppSettings<int>("Authentication:RetryAllowedCount");
            var suspendMinutes = ReadConfiguration.ReadFromAppSettings<int>("Authentication:SuspendMinutes");

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == model.Email && !u.isDeleted);
            if (user == null)
                return SetAuthenticationError(authModel, "Email or Password is incorrect!");

            if (IsUserLockedOut(user, out double remainingMinutes))
                return SetAuthenticationError(authModel, $"Your account is suspended. Try again in {Math.Ceiling(remainingMinutes)} minutes.");

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                await HandleFailedLoginAttempt(user, retryAllowedCount, suspendMinutes, userBasicInfo.IpAddress);
                return SetAuthenticationError(authModel, "Email or Password is incorrect!");
            }

            if (!user.EmailConfirmed)
                return SetAuthenticationError(authModel, "Kindly confirm your email.", false);

            return await GenerateAuthenticationResponse(user, model.RememberMe, userBasicInfo.IpAddress);
        }

        public async Task<(PaginatedList<ActivityLogDTO>? logs, int totalCount, string error)> GetAllActivityLogsAsync(ListLogsDTO query, string lang)
        {
            var items = await _activityLogingRepository.GetAllAsync(query, lang);

            if (items == null)
            {
                return (null, 0, "No items found");
            }

            return (new PaginatedList<ActivityLogDTO>(_mapper.Map<IEnumerable<ActivityLogDTO>>(items), await _activityLogingRepository.GetCountAsync(query), query.pageIndex, query.PageSize)
            , items.Count(), "");
        }




        #region Helper Functions
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            // Combine all claims
            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("uid", user.Id),
                };

            // Create the signing credentials
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            // Create the JWT token
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jwt.DurationInHours),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddHours(ReadConfiguration.ReadFromAppSettings<double>("JWT:RefreshTokenDurationInHours", 168)),
                CreatedOn = DateTime.UtcNow
            };
        }

        private bool IsUserLockedOut(ApplicationUser user, out double remainingMinutes)
        {
            remainingMinutes = 0;
            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            {
                remainingMinutes = (user.LockoutEnd.Value - DateTime.UtcNow).TotalMinutes;
                return true;
            }
            return false;
        }

        private async Task HandleFailedLoginAttempt(ApplicationUser user, int retryAllowedCount, int suspendMinutes, string ipAddress)
        {
            user.FailedLoginAttempts++;
            _ = Task.Run(async () =>
            await _activityLogingRepository.AddActivityLog(user.Id, "محاولة مرفوضة لتسجيل الدخول على النظام", "tried to login on system and got rejected", ipAddress)
            );

            if (user.FailedLoginAttempts >= retryAllowedCount)
                user.LockoutEnd = DateTime.UtcNow.AddMinutes(suspendMinutes);

            await _userManager.UpdateAsync(user);
        }

        private async Task<AuthModel> GenerateAuthenticationResponse(ApplicationUser user, bool? rememberMe, string ipAddress)
        {
            var jwtSecurityToken = await CreateJwtToken(user);
            var authModel = _mapper.Map<AuthModel>(user);

            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.TokenExpiresOn = jwtSecurityToken.ValidTo;
            authModel.IsAuthenticated = true;
            authModel.isEmailAuthonticated = user.EmailConfirmed;

            if (rememberMe == true)
            {
                var refreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive) ?? GenerateRefreshToken();
                authModel.RefreshToken = refreshToken.Token;
                authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
                if (!user.RefreshTokens.Contains(refreshToken))
                    user.RefreshTokens.Add(refreshToken);
            }

            user.lastLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            _ = Task.Run(async () =>
            await _activityLogingRepository.AddActivityLog(user.Id, "تسجيل الدخول على النظام", "login on the system", ipAddress)
            );

            return authModel;
        }

        private AuthModel SetAuthenticationError(AuthModel authModel, string error, bool isEmailAuthenticated = true)
        {
            authModel.ErrorMessage = error;
            authModel.IsAuthenticated = false;
            authModel.isEmailAuthonticated = isEmailAuthenticated;
            return authModel;
        }

        #endregion

    }
}
