using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Input;
using Models.Output;

namespace AdminTareas.BackEnd.Controllers;

[ApiController]
// [Route("api/[controller]")]
[Route("api/tasks")]
public class TaskController : ControllerBase
{
    private readonly ILogger<TaskController> _logger;
    private readonly IMediator _mediator;

    public TaskController(ILogger<TaskController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<JsonResponse>> Create(InputCreateTask request)
    {
        var response = await _mediator.Send(request);
        if (response is null)
        {
            return NotFound(new JsonResponse { Status = "Failed", Description = "Error making the request" });
        }
        return Ok(response);

    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<JsonResponse>> Update(Guid id, InputUpdateTask request)
    {
        request.Id = id;
        var response = await _mediator.Send(request);
        if (response is null)
        {
            return NotFound(new JsonResponse { Status = "Failed", Description = "Error making the request" });
        }
        return Ok(response);

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<JsonResponse>> Delete(Guid id)
    {
        var request = new InputDeleteTask { Id = id };
        var response = await _mediator.Send(request);
        if (response is null)
        {
            return NotFound(new JsonResponse { Status = "Failed", Description = "Error making the request" });
        }
        return Ok(response);

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JsonResponse>> GetById(Guid id)
    {
        var request = new InputQueryTask { Id = id };
        var response = await _mediator.Send(request);
        if (response is null)
        {
            return NotFound(new JsonResponse { Status = "Failed", Description = "Error making the request" });
        }
        return Ok(response);

    }

    [HttpGet]
    public async Task<ActionResult<JsonResponse>> GetAll()
    {
        var request = new InputGetTasks();
        var response = await _mediator.Send(request);
        if (response is null)
        {
            return NotFound(new JsonResponse { Status = "Failed", Description = "Error making the request" });
        }
        return Ok(response);

    }
}
