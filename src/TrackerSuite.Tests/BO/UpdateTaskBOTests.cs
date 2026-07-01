using Moq;
using Xunit;
using TrackerSuite.Core.Dto.Input;
using TrackerSuite.Core.Enums;
using TrackerSuite.Core.Models.Input;
using TrackerSuite.Core.Repository.IRepository.ITaskRepository;

namespace TrackerSuite.Tests.BO;

public class UpdateTaskBOTests
{
    private readonly Mock<ITaskRepository> _repositoryMock;
    private readonly UpdateTaskBO _businessObject;

    public UpdateTaskBOTests()
    {
        _repositoryMock = new Mock<ITaskRepository>();
        _businessObject = new UpdateTaskBO(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingTask_ReturnsSuccessResponse()
    {
        // Arrange
        _repositoryMock
            .Setup(repo => repo.UpdateTask(It.IsAny<TaskItem>()))
            .ReturnsAsync(TaskUpdateResult.Success);

        var request = new InputUpdateTaskDto
        {
            Id = Guid.NewGuid(),
            Title = "Updated Title",
            Description = "Updated Description",
            UserId = Guid.NewGuid()
        };

        // Act
        var response = await _businessObject.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("Success", response.Status);
        Assert.Equal("Task updated successfully", response.Description);
    }

    [Fact]
    public async Task Handle_TaskNotFound_ReturnsNotFoundResponse()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        _repositoryMock
            .Setup(repo => repo.UpdateTask(It.IsAny<TaskItem>()))
            .ReturnsAsync(TaskUpdateResult.NotFound);

        var request = new InputUpdateTaskDto { Id = taskId, Title = "Ghost Task", UserId = Guid.NewGuid() };

        // Act
        var response = await _businessObject.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("NotFound", response.Status);
        Assert.Equal($"The task with ID {taskId} does not exist", response.Description);
    }
}