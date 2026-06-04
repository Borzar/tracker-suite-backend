using MediatR;
using TrackerSuite.Core.Dto.Input;
using TrackerSuite.Core.Dto.Output;
using TrackerSuite.Core.Enums;
using TrackerSuite.Core.Repository.IRepository.ITaskRepository;

public class GetTasksBO : IRequestHandler<InputGetTasksDto, JsonResponseDto>
{
    private readonly ITaskRepository _ITaskRepository;

    public GetTasksBO(ITaskRepository ITaskRepository)
    {
        _ITaskRepository = ITaskRepository;
    }

    public async Task<JsonResponseDto> Handle(InputGetTasksDto request, CancellationToken cancellationToken)
    {
        
        try
        {

            var result = await _ITaskRepository.GetTasks(request);

            switch (result.Result)
            {
                case TaskGetTasksResult.Success:

                    var TasksList = new List<TaskItemOutputDto>(); 

                    foreach(var task in result.Task)
                    {
                        TasksList.Add(new TaskItemOutputDto
                        {
                            Id = task.Id,
                            UserId = task.UserId,
                            CategoryId = task.CategoryId,
                            Title = task.Title,
                            Description = task.Description,
                            Status = task.Status,
                            Priority = task.Priority,
                            DueDate = task.DueDate,
                            CreatedAt = task.CreatedAt,
                            UpdatedAt = task.UpdatedAt,
                        });
                    };

                    return new JsonResponseDto
                    {
                        Status = "Success",
                        Description = "Tasks fetched successfully",
                        Result = TasksList
                    };

                case TaskGetTasksResult.NotFound:
                    return new JsonResponseDto
                    {
                        Status = "NotFound",
                        Description = $"No task were found"
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
                Status = "Failed",
                Description = $"Error querying the task: {ex.Message}",
            };
        }

    }

}