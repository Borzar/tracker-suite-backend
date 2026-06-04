using MediatR;
using TrackerSuite.Core.Enums;
using TrackerSuite.Core.Dto.Input;
using TrackerSuite.Core.Dto.Output;
using TrackerSuite.Core.Models.Input;
using TrackerSuite.Core.Repository.IRepository.ITaskRepository;

public class UpdateTaskBO : IRequestHandler<InputUpdateTaskDto, JsonResponseDto>
{
    private readonly ITaskRepository _ITaskRepository;

    public UpdateTaskBO(ITaskRepository ITaskRepository)
    {
        _ITaskRepository = ITaskRepository;
    }

    public async Task<JsonResponseDto> Handle(InputUpdateTaskDto request, CancellationToken cancellationToken)
    {
        try
        {
            /* 
                Validaciones aqui 
                BadRequest para validaciones de campo
            */

            var inputTask = new TaskItem
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description
            };

            var result = await _ITaskRepository.UpdateTask(inputTask);

            switch (result)
            {
                case TaskUpdateResult.Success:
                    return new JsonResponseDto
                    {
                        Status = "Success",
                        Description = "Task updated successfully"
                    };

                case TaskUpdateResult.NotFound:
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

        } catch (Exception ex)
        {
             return new JsonResponseDto
            {
                Status = "Failed",
                Description = $"Error updating task: {ex.Message}"
            };
        }
    }
}