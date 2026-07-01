using MediatR;
using TrackerSuite.Core.Enums;
using TrackerSuite.Core.Dto.Input;
using TrackerSuite.Core.Dto.Output;
using TrackerSuite.Core.Models.Input;
using TrackerSuite.Core.Repository.IRepository.ITaskRepository;

public class CreateTaskBO : IRequestHandler<InputCreateTaskDto, JsonResponseDto>
{
    private readonly ITaskRepository _ITaskRepository;

    public CreateTaskBO(ITaskRepository ITaskRepository)
    {
        _ITaskRepository = ITaskRepository;
    }

    public async Task<JsonResponseDto> Handle(InputCreateTaskDto request, CancellationToken cancellationToken)
    {

        try
        {
            /* 
                Validaciones aqui 
                BadRequest para validaciones de campo
            */

            var inputTask = new TaskItem
            {
                Title = request.Title,
                Description = request.Description,
                UserId = request.UserId
            };

            var result = await _ITaskRepository.CreateTask(inputTask);

            switch (result)
            {
                case TaskCreateResult.Success:
                    return new JsonResponseDto
                    {
                        Status = "Success",
                        Description = "Task created successfully"
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
                Description = $"Error creating task: {ex.Message}",
            };
        }
    }

}
