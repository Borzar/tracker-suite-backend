using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackerSuite.Core.Dto.Input;
using TrackerSuite.Core.Dto.Output;

namespace TrackerSuite.Backend.Controllers;

[ApiController]
// [Route("api/[controller]")]
[Route("api/v1/tasks")]
public class TaskItemController : ControllerBase
{
    private readonly ILogger<TaskItemController> _logger;
    private readonly IMediator _mediator;
    private Guid UserIdFromToken => Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value);

    public TaskItemController(ILogger<TaskItemController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<JsonResponseDto>> Create(InputCreateTaskDto request)
    {
        request.UserId = UserIdFromToken;
        var response = await _mediator.Send(request);

        return response.Status switch
        {
            "Success" => Ok(response),
            "NotFound" => NotFound(response),
            "BadRequest" => BadRequest(response),
            _ => StatusCode(500, response)
        };

    }

    [HttpPatch("{taskId}")]
    [Authorize]
    public async Task<ActionResult<JsonResponseDto>> Update(Guid taskId, InputUpdateTaskDto request)
    {
        request.Id = taskId;
        request.UserId = UserIdFromToken;
        var response = await _mediator.Send(request);

        return response.Status switch
        {
            "Success" => Ok(response),
            "NotFound" => NotFound(response),
            "BadRequest" => BadRequest(response),
            _ => StatusCode(500, response)
        };

    }

    [HttpDelete("{taskId}")]
    [Authorize]
    public async Task<ActionResult<JsonResponseDto>> Delete(Guid taskId)
    {
        var request = new InputDeleteTaskDto { Id = taskId, UserId = UserIdFromToken };
        var response = await _mediator.Send(request);

        return response.Status switch
        {
            "Success" => Ok(response),
            "NotFound" => NotFound(response),
            "BadRequest" => BadRequest(response),
            _ => StatusCode(500, response)
        };
    }

    [HttpGet("{taskId}")]
    [Authorize]
    public async Task<ActionResult<JsonResponseDto>> GetById(Guid taskId)
    {
        var request = new InputQueryTaskDto { Id = taskId, UserId = UserIdFromToken };
        var response = await _mediator.Send(request);

        return response.Status switch
        {
            "Success" => Ok(response),
            "NotFound" => NotFound(response),
            "BadRequest" => BadRequest(response),
            _ => StatusCode(500, response)
        };

    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<JsonResponseDto>> GetAll()
    {
        var request = new InputGetTasksDto { UserId = UserIdFromToken };
        var response = await _mediator.Send(request);

        return response.Status switch
        {
            "Success" => Ok(response),
            "NotFound" => NotFound(response),
            "BadRequest" => BadRequest(response),
            _ => StatusCode(500, response)
        };

    }
}
