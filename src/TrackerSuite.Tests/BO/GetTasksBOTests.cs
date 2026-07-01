using Moq;
using Xunit;
using TrackerSuite.Core.Dto.Input;
using TrackerSuite.Core.Enums;
using TrackerSuite.Core.Models.Input;
using TrackerSuite.Core.Models.Output;
using TrackerSuite.Core.Repository.IRepository.ITaskRepository;

namespace TrackerSuite.Tests.BO;

public class GetTasksBOTests
{
    private readonly Mock<ITaskRepository> _repositoryMock;
    private readonly GetTasksBO _businessObject;

    public GetTasksBOTests()
    {
        _repositoryMock = new Mock<ITaskRepository>();
        _businessObject = new GetTasksBO(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_UserHasTasks_ReturnsTaskList()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var mockTasksList = new List<TaskItemOutput>
        {
            new() { Id = Guid.NewGuid(), UserId = userId, Title = "Task 1" },
            new() { Id = Guid.NewGuid(), UserId = userId, Title = "Task 2" }
        };

        _repositoryMock
            .Setup(repo => repo.GetTasks(It.IsAny<TaskItem>()))
            .ReturnsAsync((TaskGetTasksResult.Success, mockTasksList));

        var request = new InputGetTasksDto { UserId = userId };

        // Act
        var response = await _businessObject.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("Success", response.Status);
        Assert.Equal(2, response.Result.Count);
    }
}