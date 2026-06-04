using MediatR;
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

    public TaskItemController(ILogger<TaskItemController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<JsonResponseDto>> Create(InputCreateTaskDto request)
    {
        var response = await _mediator.Send(request);

        return response.Status switch
        {
            "Success" => Ok(response),
            "NotFound" => NotFound(response),
            "BadRequest" => BadRequest(response),
            _ => StatusCode(500, response)
        };

    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<JsonResponseDto>> Update(Guid id, InputUpdateTaskDto request)
    {
        request.Id = id;
        var response = await _mediator.Send(request);

        return response.Status switch
        {
            "Success" => Ok(response),
            "NotFound" => NotFound(response),
            "BadRequest" => BadRequest(response),
            _ => StatusCode(500, response)
        };

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<JsonResponseDto>> Delete(Guid id)
    {
        var request = new InputDeleteTaskDto { Id = id };
        var response = await _mediator.Send(request);

        return response.Status switch
        {
            "Success" => Ok(response),
            "NotFound" => NotFound(response),
            "BadRequest" => BadRequest(response),
            _ => StatusCode(500, response)
        };
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JsonResponseDto>> GetById(Guid id)
    {
        var request = new InputQueryTaskDto { Id = id };
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
    public async Task<ActionResult<JsonResponseDto>> GetAll()
    {
        var request = new InputGetTasksDto();
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
