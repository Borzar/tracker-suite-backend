using Dto.Output;
using MediatR;
using Models.Input;
using Models.Output;
using Repository.IRepository.ITaskRepository;

public class GetTasksBO : IRequestHandler<InputGetTasks, JsonResponse>
{
    private readonly ITaskRepository _ITaskRepository;

    public GetTasksBO(ITaskRepository ITaskRepository)
    {
        _ITaskRepository = ITaskRepository;
    }

    public async Task<JsonResponse> Handle(InputGetTasks request, CancellationToken cancellationToken)
    {

        var TasksList = new List<TaskOutput>(); 

        try
        {

            var resultDto = await _ITaskRepository.GetTasks(request);

            if (resultDto.ResultDto == null || resultDto.ResultDto.Count == 0)
            {
                return new JsonResponse
                {
                    Status = "Failed",
                    Description = "No tasks were found",
                    Result = new List<TaskOutput>()
                };
            }

            foreach(var tareaDto in resultDto.ResultDto)
            {
                TasksList.Add(new TaskOutput
                {
                    Id = tareaDto.IdDto,
                    Title = tareaDto.TitleDto,
                    Description = tareaDto.DescriptionDto   

                });

            };

            var result = new JsonResponse
            {
                Status = resultDto.StatusDto,
                Description = resultDto.DescriptionDto,
                Result = TasksList
            };

            return result;

        }
        catch (Exception ex)
        {
            return new JsonResponse
            {
                Status = "Failed",
                Description = $"Error querying the task: {ex.Message}",
                Result = new List<TaskOutput>()
            };
        }

    }

}