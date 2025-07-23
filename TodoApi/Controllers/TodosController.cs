using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoLibrary.DataAccess;
using TodoLibrary.Models;



namespace TodoApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TodosController : ControllerBase
{
    private readonly ITodoData _data;
    private readonly ILogger<TodosController> logger;


    public TodosController(ITodoData data, ILogger<TodosController> _logger)
    {
        _data = data;
        logger = _logger;
    }

    private int GetUserId()
    {
        var userIdText = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdText);

    }
    // GET: api/Todos 
    [HttpGet]
    public async Task<ActionResult<List<TodoModel>>> Get()
    {
        logger.LogInformation("Get: api/Togos");

        //var sdf = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        try
        {
            var output = await _data.GetAllAssigned(GetUserId());
            return Ok(output);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "the get call to api/todo failed");
            return BadRequest();
        }
    }

    // GET api/Todos/5
    [HttpGet("{todoId}")]
    public async Task<ActionResult<TodoModel>> Get(int todoId)
    {
        logger.LogInformation("Get: api/Togos/{TodoId}", todoId);
        try
        {
            var outpu = await _data.GetOneAssigned(GetUserId(), todoId);
            return Ok(outpu);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "the get call to api/todo/{APiPath} failed", $"api/Todos/{todoId}");
            return BadRequest();
        }

    }

    // POST api/Todos 
    [HttpPost]
    public async Task<ActionResult<TodoModel>> Post([FromBody] string task)
    {
        try
        {
            var output = await _data.Create(GetUserId(), task);
            return Ok(output);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"the post call to api/todo/ failed");
            return BadRequest();
        }
    }

    // PUT api/Todos/5
    [HttpPut("{todoId}")]
    public async Task<ActionResult> Put(int todoId, [FromBody] string Client_Task)
    {
        try
        {
            await _data.UpdateTask(GetUserId(), todoId, Client_Task);

            return Ok();
        }
        catch (Exception ex)
        {

            logger.LogError(ex, $"the put call to api/todo/ failed");
            return BadRequest();
        }


    }

    // PUT api/Todos/5/Complete
    [HttpPut("{todoId}/Complete")]
    public async Task<IActionResult> Complete(int todoId)
    {
        try
        {
            await _data.CompleteTodo(GetUserId(), todoId);

            return Ok();
        }
        catch (Exception ex)
        {

            logger.LogError(ex, $"the Put complete call to api/todo/ failed");
            return BadRequest();
        }


    }

    // DELETE api/Todos/5
    [HttpDelete("{todoId}")]
    public async Task<IActionResult> Delete(int todoId)
    {
        try
        {
            await _data.Delete(GetUserId(), todoId);
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"the delete call to api/todo/ failed");
            return BadRequest();
        }

    }
}
