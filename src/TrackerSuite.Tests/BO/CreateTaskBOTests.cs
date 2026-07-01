using Moq;
using Xunit;
using TrackerSuite.Core.Dto.Input;
using TrackerSuite.Core.Enums;
using TrackerSuite.Core.Models.Input;
using TrackerSuite.Core.Repository.IRepository.ITaskRepository;

namespace TrackerSuite.Tests.BO;

public class CreateTaskBOTests
{
    private readonly Mock<ITaskRepository> _repositoryMock;
    private readonly CreateTaskBO _businessObject;

    public CreateTaskBOTests()
    {
        _repositoryMock = new Mock<ITaskRepository>();
        _businessObject = new CreateTaskBO(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidTask_ReturnsSuccessResponse()
    {
        // Arrange
        _repositoryMock
            .Setup(repo => repo.CreateTask(It.IsAny<TaskItem>()))
            .ReturnsAsync(TaskCreateResult.Success);

        var request = new InputCreateTaskDto
        {
            Title = "Finish backend unit tests",
            Description = "Write xUnit tests for all BOs",
            UserId = Guid.NewGuid()
        };

        // Act
        var response = await _businessObject.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("Success", response.Status);
        Assert.Equal("Task created successfully", response.Description);
        _repositoryMock.Verify(repo => repo.CreateTask(It.IsAny<TaskItem>()), Times.Once);
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_ReturnsFailedResponse()
    {
        // Arrange
        _repositoryMock
            .Setup(repo => repo.CreateTask(It.IsAny<TaskItem>()))
            .ThrowsAsync(new Exception("Database connection timeout"));

        var request = new InputCreateTaskDto { Title = "Faulty Task", UserId = Guid.NewGuid() };

        // Act
        var response = await _businessObject.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("Failed", response.Status);
        Assert.Contains("Error creating task: Database connection timeout", response.Description);
    }
}