using MediatR;
using TrackerSuite.Core.Enums;
using TrackerSuite.Core.Dto.Input;
using TrackerSuite.Core.Dto.Output;
using TrackerSuite.Core.Models.Input;
using TrackerSuite.Core.Repository.IRepository.ITaskRepository;

public class GetByIdBO : IRequestHandler<InputQueryTaskDto, JsonResponseDto>
{
    private readonly ITaskRepository _ITaskRepository;

    public GetByIdBO(ITaskRepository ITaskRepository)
    {
        _ITaskRepository = ITaskRepository;
    }

    public async Task<JsonResponseDto> Handle(InputQueryTaskDto request, CancellationToken cancellationToken)
    {

        var inputTask = new TaskItem();

        try
        {
            inputTask.Id = request.Id;
            inputTask.UserId = request.UserId;

            var result = await _ITaskRepository.GetById(inputTask);

            switch (result.Result)
            {
                case TaskGetByIdResult.Success:
                    var tasks = new TaskItemOutputDto
                    {
                        Id = result.Task[0].Id,
                        UserId = result.Task[0].UserId,
                        CategoryId = result.Task[0].CategoryId,
                        Title = result.Task[0].Title,
                        Description = result.Task[0].Description,
                        Status = result.Task[0].Status,
                        Priority = result.Task[0].Priority,
                        DueDate = result.Task[0].DueDate,
                        CreatedAt = result.Task[0].CreatedAt,
                        UpdatedAt = result.Task[0].UpdatedAt,
                    };

                    return new JsonResponseDto
                    {
                        Status = "Success",
                        Description = "Task retrieved successfully",
                        Result = new List<TaskItemOutputDto> { tasks }
                    };

                case TaskGetByIdResult.NotFound:
                    return new JsonResponseDto
                    {
                        Status = "NotFound",
                        Description = $"The task with ID {request.Id} does not exist"
                    };

                default:
                    return new JsonResponseDto
                    {
                        Status = "Failed",
                        Description = "Unknown error"
                    };
            }
        }
        catch (Exception ex)
        {
            return new JsonResponseDto
            {
                Status = "Error",
                Description = $"Error querying the task: {ex.Message}",
            };
        }

    }

}