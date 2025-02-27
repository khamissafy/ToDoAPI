using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using ToDoListAPI.Helpers;
using ToDoListAPI.Models.ToDoListManagement.DB_Models;
using ToDoListAPI.Models.ToDoListManagement.DTOs;
using ToDoListAPI.Models.UserManagement.DB_Models;
using ToDoListAPI.Models.UserManagement.DTOs;
using ToDoListAPI.Services.ActivityLoging.Interfaces;
using ToDoListAPI.Services.ToDoList.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly IMapper _mapper;
    private readonly IActivityLogingRepository _activityLogingService;

    public TodoService(ITodoRepository repository, IMemoryCache cache, IMapper mapper, IActivityLogingRepository activityLogingService)
    {
        _repository = repository;
        _cache = cache;
        _mapper = mapper;
        _activityLogingService = activityLogingService;
    }

    public async Task<(PaginatedList<TodoItemDto>? todos, int totalCount, string error)> GetAllAsync(ListTodoDTO query)
    {
        var items = await _repository.GetAllAsync(query);

        if (items == null)
        {
            return (null, 0, "No items found");
        }

        return (new PaginatedList<TodoItemDto>(_mapper.Map<IEnumerable<TodoItemDto>>(items), await _repository.GetCountAsync(query), query.pageIndex, query.PageSize)
        , items.Count(), "");
    }

    public async Task<(TodoItemDto? todo, string error)> GetByIdAsync(Guid id)
    {
        var item = await _cache.GetOrCreateAsync($"todo_item_{id}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
            return await _repository.GetByIdAsync(id);
        });

        if (item == null)   
        {
            return (null, "Item not found");
        }

        return (_mapper.Map<TodoItemDto>(item), "");
    }

    public async Task<(bool success, string error)> AddAsync(CreateTodoDto todoDto, UserBasicInfo actionTakenBy)
    {
        var todo = _mapper.Map<TodoItem>(todoDto);
        todo.Id = Guid.NewGuid();
        await _repository.AddAsync(todo);

        _ = Task.Run(async () => 
            await _activityLogingService.AddActivityLog(actionTakenBy.UserId, $"اضافة عنصر جديد العنوان الخاص به هو '{todoDto.Title}'", $"added new Todo item, the Title is '{todoDto.Title}'", actionTakenBy.IpAddress)
        );

        return (true, "");
    }

    public async Task<(bool success, string error)> UpdateAsync(Guid id, UpdateTodoDto todoDto, UserBasicInfo actionTakenBy)
    {
        var todo = await _repository.GetByIdAsync(id);
        if (todo == null)
        {
            return (false, "Item not found");
        }

        _mapper.Map(todoDto, todo);
        await _repository.UpdateAsync(todo);

        _ = Task.Run(async () =>
        await _activityLogingService.AddActivityLog(actionTakenBy.UserId, $"تعديل عنصر حالي المعرف الخاص به هو '{id}'", $"updated existing item, the ID is '{id}'", actionTakenBy.IpAddress)
        );

        return (true, "");
    }

    public async Task<(bool success, string error)> SoftDeleteAsync(Guid id, UserBasicInfo actionTakenBy)
    {
        var todo = await _repository.GetByIdAsync(id);
        if (todo == null)
        {
            return (false, "Item not found");
        }
        todo.IsDeleted = true;
        await _repository.UpdateAsync(todo);

        _ = Task.Run(async () =>
        await _activityLogingService.AddActivityLog(actionTakenBy.UserId, $"حذف عنصر ,المعرف الخاص به هو '{todo.Title}'", $"deleted item, the ID is '{todo.Title}'", actionTakenBy.IpAddress)
        );


        return (true, "");
    }
}
