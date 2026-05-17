using AdminTasks.Backend.Core.Models;
using MediatR;
using Models.Input;
using Models.Output;
using Repository.IRepository.ITaskRepository;

public class DeleteTaskBO : IRequestHandler<InputDeleteTask, JsonResponse>
{
    private readonly ITaskRepository _ITaskRepository;

    public DeleteTaskBO(ITaskRepository ITaskRepository)
    {
        _ITaskRepository = ITaskRepository;
    }

    public async Task<JsonResponse> Handle(InputDeleteTask request, CancellationToken cancellationToken)
    {

        var inputTask = new TaskItem();

        try
        {
            inputTask.Id = request.Id;

            var resultDto = await _ITaskRepository.DeleteTask(inputTask);

            if (resultDto == null)
            {
                return new JsonResponse
                {
                    Status = "Failed",
                    Description = $"The task with ID {request.Id} does not exist",
                    Result = new List<TaskOutput>()
                };
            }

            return new JsonResponse
            {
                Status = resultDto.StatusDto,
                Description = resultDto.DescriptionDto,
                Result = new List<TaskOutput>()

            };

        }
        catch (Exception ex)
        {
            return new JsonResponse
            {
                Status = "Failed",
                Description = $"Error deleting task: {ex.Message}",
                Result = new List<TaskOutput>()
            };
        }
       
    }

}