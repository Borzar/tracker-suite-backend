using AdminTasks.Backend.Core.Models;
using MediatR;
using Models.Input;
using Models.Output;
using Repository.IRepository.ITaskRepository;

public class UpdateTaskBO : IRequestHandler<InputUpdateTask, JsonResponse>
{
    private readonly ITaskRepository _ITaskRepository;

    public UpdateTaskBO(ITaskRepository ITaskRepository)
    {
        _ITaskRepository = ITaskRepository;
    }

    public async Task<JsonResponse> Handle(InputUpdateTask request, CancellationToken cancellationToken)
    {

        var inputTask = new TaskItem();

        try
        {

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                inputTask.Title = request.Title.Trim();
            };

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                inputTask.Description = request.Description.Trim();
            };

            inputTask.Id = request.Id;

            var resultDto = await _ITaskRepository.UpdateTask(inputTask);

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

        } catch (Exception ex)
        {
            return new JsonResponse
            {
                Status = "Fallido",
                Description = $"Error updating task: {ex.Message}",
                Result = new List<TaskOutput>()
            };
        }
    }
}