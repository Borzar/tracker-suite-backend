using Moq;
using Xunit;
using TrackerSuite.Core.Dto.Input;
using TrackerSuite.Core.Enums;
using TrackerSuite.Core.Models.Input;
using TrackerSuite.Core.Repository.IRepository.ITaskRepository;

namespace TrackerSuite.Tests.BO;

public class DeleteTaskBOTests
{
    private readonly Mock<ITaskRepository> _repositoryMock;
    private readonly DeleteTaskBO _businessObject;

    public DeleteTaskBOTests()
    {
        _repositoryMock = new Mock<ITaskRepository>();
        _businessObject = new DeleteTaskBO(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingTask_ReturnsSuccessResponse()
    {
        // Arrange
        _repositoryMock
            .Setup(repo => repo.DeleteTask(It.IsAny<TaskItem>()))
            .ReturnsAsync(TaskDeleteResult.Success);

        var request = new InputDeleteTaskDto { Id = Guid.NewGuid(), UserId = Guid.NewGuid() };

        // Act
        var response = await _businessObject.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("Success", response.Status);
        Assert.Equal("Task deleted successfully", response.Description);
    }

    [Fact]
    public async Task Handle_TaskNotFound_ReturnsNotFoundResponse()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        _repositoryMock
            .Setup(repo => repo.DeleteTask(It.IsAny<TaskItem>()))
            .ReturnsAsync(TaskDeleteResult.NotFound);

        var request = new InputDeleteTaskDto { Id = taskId, UserId = Guid.NewGuid() };

        // Act
        var response = await _businessObject.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("NotFound", response.Status);
        Assert.Equal($"The task with ID {taskId} does not exist", response.Description);
    }
}