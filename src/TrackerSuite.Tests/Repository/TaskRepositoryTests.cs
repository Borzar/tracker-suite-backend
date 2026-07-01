using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrackerSuite.Core.Data;
using TrackerSuite.Core.Enums;
using TrackerSuite.Core.Models.Input;
using TrackerSuite.Core.Repository.TaskRepository;
using Xunit;

namespace TrackerSuite.Tests.Repository;

public class TaskRepositoryTests
{
    // Método auxiliar para generar un DbContext limpio en memoria en cada test
    private TaskDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<TaskDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Nombre único por test
            .Options;

        return new TaskDbContext(options);
    }

    [Fact]
    public async Task CreateTask_ValidTask_SavesCorrectlyInDatabase()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new TaskRepository(context);

        var newTask = new TaskItem
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Title = "Database Integration Test",
            Description = "Testing the repository layer"
        };

        // Act
        var result = await repository.CreateTask(newTask);

        // Assert
        Assert.Equal(TaskCreateResult.Success, result);
        
        // Verificar que realmente se guardó en la "base de datos"
        var taskInDb = await context.Tasks.FirstOrDefaultAsync(t => t.Id == newTask.Id);
        Assert.NotNull(taskInDb);
        Assert.Equal("Database Integration Test", taskInDb.Title);
    }

    [Fact]
    public async Task UpdateTask_ExistingTask_UpdatesFieldsCorrectly()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new TaskRepository(context);

        var taskId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var existingTask = new TaskItem
        {
            Id = taskId,
            UserId = userId,
            Title = "Old Title",
            Description = "Old Description"
        };

        await context.Tasks.AddAsync(existingTask);
        await context.SaveChangesAsync();

        var updatedTaskInput = new TaskItem
        {
            Id = taskId,
            UserId = userId,
            Title = "   New Beautiful Title   ", // Trimming verification
            Description = "New Description"
        };

        // Act
        var result = await repository.UpdateTask(updatedTaskInput);

        // Assert
        Assert.Equal(TaskUpdateResult.Success, result);

        var taskInDb = await context.Tasks.FindAsync(taskId);
        Assert.Equal("New Beautiful Title", taskInDb.Title); // Verifies that .Trim() worked
        Assert.Equal("New Description", taskInDb.Description);
    }

    [Fact]
    public async Task UpdateTask_TaskDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new TaskRepository(context);

        var input = new TaskItem
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Title = "Non-existent Task"
        };

        // Act
        var result = await repository.UpdateTask(input);

        // Assert
        Assert.Equal(TaskUpdateResult.NotFound, result);
    }

    [Fact]
    public async Task DeleteTask_ExistingTask_RemovesFromDatabase()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new TaskRepository(context);

        var taskId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var existingTask = new TaskItem { Id = taskId, UserId = userId, Title = "To Be Deleted" };

        await context.Tasks.AddAsync(existingTask);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.DeleteTask(existingTask);

        // Assert
        Assert.Equal(TaskDeleteResult.Success, result);
        
        var taskInDb = await context.Tasks.FindAsync(taskId);
        Assert.Null(taskInDb); // Verifies it no longer exists
    }

    [Fact]
    public async Task GetTasks_UserHasMultipleTasks_ReturnsOnlyUserTasks()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new TaskRepository(context);

        var targetUserId = Guid.NewGuid();
        var foreignUserId = Guid.NewGuid();

        await context.Tasks.AddRangeAsync(new List<TaskItem>
        {
            new() { Id = Guid.NewGuid(), UserId = targetUserId, Title = "Target User Task 1" },
            new() { Id = Guid.NewGuid(), UserId = targetUserId, Title = "Target User Task 2" },
            new() { Id = Guid.NewGuid(), UserId = foreignUserId, Title = "Foreign User Task" }
        });
        await context.SaveChangesAsync();

        var input = new TaskItem { UserId = targetUserId };

        // Act
        var (result, tasks) = await repository.GetTasks(input);

        // Assert
        Assert.Equal(TaskGetTasksResult.Success, result);
        Assert.Equal(2, tasks.Count); // Verifies only 2 tasks belonging to targetUserId are fetched
        Assert.All(tasks, t => Assert.Equal(targetUserId, t.UserId));
    }
}