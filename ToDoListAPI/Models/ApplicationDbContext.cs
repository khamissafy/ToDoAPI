using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Models.ToDoListManagement.DB_Models;
using ToDoListAPI.Models.UserManagement.DB_Models;

namespace ToDoListAPI.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        public DbSet<ActivityLog> ActivityLogs {  get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }

    }
}
