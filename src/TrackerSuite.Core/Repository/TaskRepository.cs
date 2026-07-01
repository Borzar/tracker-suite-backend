using Microsoft.EntityFrameworkCore;
using TrackerSuite.Core.Enums;
using TrackerSuite.Core.Data;
using TrackerSuite.Core.Models.Input;
using TrackerSuite.Core.Models.Output;
using TrackerSuite.Core.Repository.IRepository.ITaskRepository;
using TrackerSuite.Core.Dto.Input;

namespace TrackerSuite.Core.Repository.TaskRepository;

public class TaskRepository : ITaskRepository
{
    private readonly TaskDbContext _context;

    public TaskRepository(TaskDbContext context)
    {
        _context = context;
    }

    public async Task<TaskCreateResult> CreateTask(TaskItem input)
    {
        await _context.Tasks.AddAsync(input);
        await _context.SaveChangesAsync();
        return TaskCreateResult.Success;
    }
    

    public async Task<TaskUpdateResult> UpdateTask(TaskItem input)
    {
        var existingTask = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == input.Id && t.UserId == input.UserId);

        if (existingTask == null)
            return TaskUpdateResult.NotFound;

        if (!string.IsNullOrWhiteSpace(input.Title))
            existingTask.Title = input.Title.Trim();

        if (!string.IsNullOrWhiteSpace(input.Description))
            existingTask.Description = input.Description.Trim();

        await _context.SaveChangesAsync();

        return TaskUpdateResult.Success;
    }

    public async Task<TaskDeleteResult> DeleteTask(TaskItem input)
    {
        var existingTask = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == input.Id && t.UserId == input.UserId);

        if (existingTask == null)
            return TaskDeleteResult.NotFound;

        _context.Tasks.Remove(existingTask);

        await _context.SaveChangesAsync();

        return TaskDeleteResult.Success;
  
    }

    public async Task<(TaskGetByIdResult Result, List<TaskItemOutput> Task)> GetById(TaskItem input)
    {
        var existingTask = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == input.Id && t.UserId == input.UserId);

        if (existingTask == null)
            return (TaskGetByIdResult.NotFound, null);
        
        var taskById = new TaskItemOutput
        {
            Id = existingTask.Id,
            UserId = existingTask.UserId,
            CategoryId = existingTask.CategoryId,
            Title = existingTask.Title,
            Description = existingTask.Description,
            Status = existingTask.Status,
            Priority = existingTask.Priority,
            DueDate = existingTask.DueDate,
            CreatedAt = existingTask.CreatedAt,
            UpdatedAt = existingTask.UpdatedAt
        };

        return (TaskGetByIdResult.Success, new List<TaskItemOutput> { taskById });

    }

    public async Task<(TaskGetTasksResult Result, List<TaskItemOutput> Task)> GetTasks(TaskItem input)
    {
        var taskList = await _context.Tasks.Where(t => t.UserId == input.UserId).ToListAsync();

        if (taskList == null || taskList.Count == 0)
            return (TaskGetTasksResult.NotFound, null);

        var tasks = new List<TaskItemOutput>(); 

        foreach (var taskItem in taskList)
        {
            tasks.Add(new TaskItemOutput
            {
                Id= taskItem.Id,
                UserId = taskItem.UserId,
                CategoryId = taskItem.CategoryId,
                Title = taskItem.Title,
                Description = taskItem.Description,
                Status = taskItem.Status,
                Priority = taskItem.Priority,
                DueDate = taskItem.DueDate,
                CreatedAt = taskItem.CreatedAt,
                UpdatedAt = taskItem.UpdatedAt
            });
        };

        return (TaskGetTasksResult.Success, tasks );

    }

}