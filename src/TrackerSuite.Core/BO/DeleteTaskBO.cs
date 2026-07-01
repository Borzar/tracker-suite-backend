using MediatR;
using TrackerSuite.Core.Enums;
using TrackerSuite.Core.Models.Input;
using TrackerSuite.Core.Dto.Input;
using TrackerSuite.Core.Dto.Output;
using TrackerSuite.Core.Repository.IRepository.ITaskRepository;

public class DeleteTaskBO : IRequestHandler<InputDeleteTaskDto, JsonResponseDto>
{
    private readonly ITaskRepository _ITaskRepository;

    public DeleteTaskBO(ITaskRepository ITaskRepository)
    {
        _ITaskRepository = ITaskRepository;
    }

    public async Task<JsonResponseDto> Handle(InputDeleteTaskDto request, CancellationToken cancellationToken)
    {

        try
        {

            var inputTask = new TaskItem
            {
                Id = request.Id,
                UserId = request.UserId
            };

            var result = await _ITaskRepository.DeleteTask(inputTask);

            switch (result)
            {
                case TaskDeleteResult.Success:
                    return new JsonResponseDto
                    {
                        Status = "Success",
                        Description = "Task deleted successfully"
                    };

                case TaskDeleteResult.NotFound:
                    return new JsonResponseDto
                    {
                        Status = "NotFound",
                        Description = $"The task with ID {request.Id} does not exist",
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
                Description = $"Error deleting task: {ex.Message}",
            };
        }
       
    }

}