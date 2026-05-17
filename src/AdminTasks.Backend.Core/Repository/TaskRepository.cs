using System.Runtime.InteropServices;
using AdminTasks.Backend.Core.Models;
using Dto.Output;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Models.Input;
using Repository.IRepository.ITaskRepository;
namespace Repository.TaskRepository;

public class TaskRepository : ITaskRepository
{
    private readonly TaskDbContext _context;

    public TaskRepository(TaskDbContext context)
    {
        _context = context;
    }

    public async Task<JsonResponseDto> CreateTask(TaskItem inputDto)
    {
        await _context.Tasks.AddAsync(inputDto);
        await _context.SaveChangesAsync();
        return new JsonResponseDto { StatusDto = "Success", DescriptionDto = "", ResultDto = new List<TaskOutputDto>() };
    }
    

    public async Task<JsonResponseDto> UpdateTask(TaskItem inputDto)
    {
        var existingTaskDto = await _context.Tasks.FindAsync(inputDto.Id);

        if (existingTaskDto == null)
        {
            return new JsonResponseDto
            {
                StatusDto = "Failed",
                DescriptionDto = $"The task with ID {inputDto.Id} does not exist",
                ResultDto = new List<TaskOutputDto>()
            };
        }

        if (!string.IsNullOrWhiteSpace(inputDto.Title))
        {
            existingTaskDto.Title = inputDto.Title.Trim();
        };

        if (!string.IsNullOrWhiteSpace(inputDto.Description))
        {
            existingTaskDto.Description = inputDto.Description.Trim();
        };

        await _context.SaveChangesAsync();
        return new JsonResponseDto { StatusDto = "Success", DescriptionDto = "", ResultDto = new List<TaskOutputDto>() };
    }

    public async Task<JsonResponseDto> DeleteTask(TaskItem inputDto)
    {
        var existingTaskDto = await _context.Tasks.FindAsync(inputDto.Id);
        _context.Tasks.Remove(existingTaskDto);
        await _context.SaveChangesAsync();

        return new JsonResponseDto { StatusDto = "Success", DescriptionDto = "", ResultDto = new List<TaskOutputDto>() };
    }

    public async Task<JsonResponseDto> QueryTask(TaskItem inputDto)
    {
        var existingTaskDto = await _context.Tasks.FindAsync(inputDto.Id);

        var taskDto = new TaskOutputDto
        {
            IdDto = existingTaskDto.Id,
            TitleDto = existingTaskDto.Title,
            DescriptionDto = existingTaskDto.Description
        };

        return new JsonResponseDto { StatusDto = "Success", DescriptionDto = "", ResultDto = new List<TaskOutputDto> { taskDto } };
    }

    public async Task<JsonResponseDto> GetTasks(InputGetTasks inputDto)
    {
        var tasksDto = new List<TaskOutputDto>(); 

        var taskListDto = await _context.Tasks.ToListAsync();

        foreach (var tarea in taskListDto)
        {
            tasksDto.Add(new TaskOutputDto
            {
                IdDto = tarea.Id,
                TitleDto = tarea.Title,
                DescriptionDto = tarea.Description
            });

        };

        return new JsonResponseDto { StatusDto = "Success", DescriptionDto = "", ResultDto =  tasksDto };
    }

}