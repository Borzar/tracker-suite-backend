using TrackerSuite.Core.Dto.Input;
using TrackerSuite.Core.Enums;
using TrackerSuite.Core.Models.Input;
using TrackerSuite.Core.Models.Output;

namespace TrackerSuite.Core.Repository.IRepository.ITaskRepository;

public interface ITaskRepository
{
    public Task<TaskCreateResult> CreateTask(TaskItem input);
    public Task<TaskUpdateResult> UpdateTask(TaskItem input);
    public Task<TaskDeleteResult> DeleteTask(TaskItem input);
    public Task<(TaskGetByIdResult Result, List<TaskItemOutput> Task)> GetById(TaskItem input);
    public Task<(TaskGetTasksResult Result, List<TaskItemOutput> Task)> GetTasks(InputGetTasksDto input);
}