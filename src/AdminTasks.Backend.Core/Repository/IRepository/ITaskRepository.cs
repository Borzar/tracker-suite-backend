using AdminTasks.Backend.Core.Models;
using Dto.Output;
using Models.Input;

namespace Repository.IRepository.ITaskRepository;

public interface ITaskRepository
{
    public Task<JsonResponseDto> CreateTask(TaskItem inputDto);
    public Task<JsonResponseDto> UpdateTask(TaskItem inputDto);
    public Task<JsonResponseDto> DeleteTask(TaskItem inputDto);
    public Task<JsonResponseDto> QueryTask(TaskItem inputDto);
    public Task<JsonResponseDto> GetTasks(InputGetTasks inputDto);
}