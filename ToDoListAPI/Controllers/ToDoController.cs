using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListAPI.Helpers;
using ToDoListAPI.Models.ToDoListManagement.DTOs;
using ToDoListAPI.Services.ToDoList.Interfaces;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ToDoController : ControllerBase
{
    private readonly ITodoService _service;

    public ToDoController(ITodoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ListTodoDTO query)
    {
        var (todos, total, error) = await _service.GetAllAsync(query);
        if (!string.IsNullOrEmpty(error))
            return BadRequest(new APIResponse { ErrorMessage = error });

        return Ok(new APIResponse { Data = todos, Count = total });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var (item, error) = await _service.GetByIdAsync(id);
        if (!string.IsNullOrEmpty(error))
            return NotFound(new APIResponse { ErrorMessage =  error });

        return Ok(new APIResponse { Data = item });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTodoDto todo)
    {
        var userInfo = UserBasicInfoHandler.GetUserBasicInfo(HttpContext, User);

        var (success, error) = await _service.AddAsync(todo, userInfo);
        if (!success)
            return BadRequest(new APIResponse { ErrorMessage = error });

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTodoDto todo)
    {
        var userInfo = UserBasicInfoHandler.GetUserBasicInfo(HttpContext, User);

        var (success, error) = await _service.UpdateAsync(id, todo, userInfo);
        if (!success)
            return BadRequest(new APIResponse { ErrorMessage = error });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userInfo = UserBasicInfoHandler.GetUserBasicInfo(HttpContext, User);

        var (success, error) = await _service.SoftDeleteAsync(id, userInfo);
        if (!success)
            return BadRequest(new APIResponse { ErrorMessage = error });

        return NoContent();
    }
}
