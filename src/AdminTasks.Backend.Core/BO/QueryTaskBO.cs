using AdminTasks.Backend.Core.Models;
using Dto.Output;
using MediatR;
using Models.Input;
using Models.Output;
using Repository.IRepository.ITaskRepository;

public class QueryTaskBO : IRequestHandler<InputQueryTask, JsonResponse>
{
    private readonly ITaskRepository _ITaskRepository;

    public QueryTaskBO(ITaskRepository ITaskRepository)
    {
        _ITaskRepository = ITaskRepository;
    }

    public async Task<JsonResponse> Handle(InputQueryTask request, CancellationToken cancellationToken)
    {

        var inputTask = new TaskItem();

        try
        {
            inputTask.Id = request.Id;

            var resultDto = await _ITaskRepository.QueryTask(inputTask);

            if (resultDto == null)
            {
                return new JsonResponse
                {
                    Status = "Failed",
                    Description = $"The task with ID {request.Id} does not exist",
                    Result = new List<TaskOutput>()
                };
            }

            if (resultDto.ResultDto == null || resultDto.ResultDto.Count == 0)
            {
                return new JsonResponse
                {
                    Status = "Failed",
                    Description = "No tasks found",
                    Result = new List<TaskOutput>()
                };
            }

            var tasks = new TaskOutput
            {
                Id = resultDto.ResultDto[0].IdDto,
                Title = resultDto.ResultDto[0].TitleDto,
                Description = resultDto.ResultDto[0].DescriptionDto

            };

            return new JsonResponse
            {
                Status = resultDto.StatusDto,
                Description = resultDto.DescriptionDto,
                Result = new List<TaskOutput> { tasks }
            };  

        
        }
        catch (Exception ex)
        {
            return new JsonResponse
            {
                Status = "Error",
                Description = $"Error querying the task: {ex.Message}",
                Result = new List<TaskOutput>()
            };
        }

    }

}