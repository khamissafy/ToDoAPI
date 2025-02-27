using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Helpers;
using ToDoListAPI.Models;
using ToDoListAPI.Models.ToDoListManagement.DTOs;
using ToDoListAPI.Models.UserManagement.DB_Models;
using ToDoListAPI.Models.UserManagement.DTOs;
using ToDoListAPI.Services.ActivityLoging.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ToDoListAPI.Services.ActivityLoging.Classes
{
    public class ActivityLogingRepository : IActivityLogingRepository
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ActivityLogingRepository> _logger;

        public ActivityLogingRepository(IServiceScopeFactory serviceScopeFactory, ILogger<ActivityLogingRepository> logger, ApplicationDbContext context)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _context = context;
        }

        public async Task AddActivityLog(string? userId, string descriptionAr, string descriptionEn, string? ipAddress)
        {
            if (userId == null)
                return;
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var user = await dbContext.Users.FindAsync(userId);
            if (user == null)
                return;
            try
            {
                var preparedDescriptionAr = $"المستخدم '{user.FullName}' قام ب{descriptionAr}";
                var preparedDescriptionEn = $"the user '{user.FullName}' has {descriptionEn}";

                var activityLog = new ActivityLog
                {
                    ActionPerformed = preparedDescriptionEn,
                    ActionPerformedAr = preparedDescriptionAr,
                    IpAddress = ipAddress,
                    UserId = user.Id
                };

                await dbContext.ActivityLogs.AddAsync(activityLog);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding activity log for user {UserId}", user.Id);
            }
        }

        public async Task<IEnumerable<ActivityLogDTO>> GetAllAsync(ListLogsDTO dto, string lang)
        {
            var logsQuery = GetActivityLogsQuery(dto);

            // Ordering based on 'OrderedBy' parameter
            switch (dto.OrderedBy?.ToLower())
            {
                case "performedat_desc":
                    logsQuery = logsQuery.OrderByDescending(l => l.PerformedAt);
                    break;
                case "performedat":
                    logsQuery = logsQuery.OrderBy(l => l.PerformedAt);
                    break;
                case "actionperformed_desc":
                    logsQuery = logsQuery.OrderByDescending(l => l.ActionPerformed);
                    break;
                case "actionperformed":
                    logsQuery = logsQuery.OrderBy(l => l.ActionPerformed);
                    break;
                case "actionperformedar_desc":
                    logsQuery = logsQuery.OrderByDescending(l => l.ActionPerformed);
                    break;
                case "actionperformedar":
                    logsQuery = logsQuery.OrderBy(l => l.ActionPerformed);
                    break;
                case "userid_desc":
                    logsQuery = logsQuery.OrderByDescending(l => l.UserId);
                    break;
                case "userid":
                    logsQuery = logsQuery.OrderBy(l => l.UserId);
                    break;
                default:
                    logsQuery = logsQuery.OrderByDescending(l => l.PerformedAt);
                    break;
            }

            var paginatedLogs = await logsQuery
                .Skip((dto.pageIndex - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .Join(_context.Users, log => log.UserId, user => user.Id, (log, user) => new ActivityLogDTO
                {
                    ActionPerformed = lang == "ar" ? log.ActionPerformedAr : log.ActionPerformed,
                    IpAddress = log.IpAddress,
                    PerformedAt = log.PerformedAt,
                    UserId = log.UserId,
                    UserName = user.FullName
                })
                .ToListAsync();

            return paginatedLogs;
            
        }

        public async Task<int> GetCountAsync(ListLogsDTO dto)
        {
            var query = GetActivityLogsQuery(dto);

            return await query.CountAsync();

        }

        #region Helper Functions
        private IQueryable<ActivityLog> GetActivityLogsQuery(ListLogsDTO dto)
        {
            var logsQuery = _context.ActivityLogs.AsQueryable();
            // Filtering by search term
            if (!string.IsNullOrEmpty(dto.SearchTerm))
            {
                logsQuery = logsQuery.Where(l => l.ActionPerformed.Contains(dto.SearchTerm) || l.ActionPerformedAr.Contains(dto.SearchTerm));
            }

            // Filtering by performed date range
            if (dto.StartDate.HasValue)
            {
                logsQuery = logsQuery.Where(l => l.PerformedAt >= dto.StartDate);
            }
            if (dto.EndDate.HasValue)
            {
                logsQuery = logsQuery.Where(l => l.PerformedAt <= dto.EndDate);
            }

            if (!string.IsNullOrEmpty(dto.UserId))
            {
                logsQuery = logsQuery.Where(l => l.UserId == dto.UserId);
            }

            return logsQuery;
        }

        #endregion
    }
}
