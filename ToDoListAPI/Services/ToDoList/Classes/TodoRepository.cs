using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Models;
using ToDoListAPI.Models.ToDoListManagement.DB_Models;
using ToDoListAPI.Models.ToDoListManagement.DTOs;
using ToDoListAPI.Services.ToDoList.Interfaces;

public class TodoRepository : ITodoRepository
{
    private readonly ApplicationDbContext _context;

    public TodoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoItem>> GetAllAsync(ListTodoDTO dto)
    {
        var query = GetTodoItemQuery(dto);
        
        if (!string.IsNullOrWhiteSpace(dto.OrderedBy))
        {
            query = dto.OrderedBy.ToLower() switch
            {
                "title" => query.OrderBy(t => t.Title),
                "title_desc" => query.OrderByDescending(t => t.Title),
                "description" => query.OrderBy(t => t.Title),
                "description_desc" => query.OrderByDescending(t => t.Title),
                "createddate" => query.OrderBy(t => t.CreatedAt),
                "createddate_desc" => query.OrderByDescending(t => t.CreatedAt),
                _ => query.OrderBy(t => t.Title)
            };
        }

        query = query.Skip((dto.pageIndex - 1) * dto.PageSize)
            .Take(dto.PageSize);

        return await query.ToListAsync();
    }

    public async Task<int> GetCountAsync(ListTodoDTO dto)
    {
        var query = GetTodoItemQuery(dto);

        return await query.CountAsync();
    }


    public async Task<TodoItem?> GetByIdAsync(Guid id)
    {
        return await _context.TodoItems.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
    }

    public async Task AddAsync(TodoItem todo)
    {
        await _context.TodoItems.AddAsync(todo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TodoItem todo)
    {
        _context.TodoItems.Update(todo);
        await _context.SaveChangesAsync();
    }

    #region Helper Function
    private IQueryable<TodoItem> GetTodoItemQuery(ListTodoDTO dto)
    {
        var query = _context.TodoItems.Where(t => !t.IsDeleted).AsQueryable();

        if (!string.IsNullOrWhiteSpace(dto.SearchTerm))
        {
            query = query.Where(t => t.Title.Contains(dto.SearchTerm) || t.Description.Contains(dto.SearchTerm));
        }
        return query;
    }
    #endregion
}
