using Moq;
using Xunit;
using TrackerSuite.Core.Dto.Input;
using TrackerSuite.Core.Enums;
using TrackerSuite.Core.Models.Input;
using TrackerSuite.Core.Models.Output;
using TrackerSuite.Core.Repository.IRepository.ITaskRepository;

namespace TrackerSuite.Tests.BO;

public class GetByIdBOTests
{
    private readonly Mock<ITaskRepository> _repositoryMock;
    private readonly GetByIdBO _businessObject;

    public GetByIdBOTests()
    {
        _repositoryMock = new Mock<ITaskRepository>();
        _businessObject = new GetByIdBO(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_TaskExists_ReturnsTaskPayload()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var mockDbOutput = new TaskItemOutput
        {
            Id = taskId,
            UserId = userId,
            Title = "Test Task",
            Description = "Description Example"
        };

        _repositoryMock
            .Setup(repo => repo.GetById(It.IsAny<TaskItem>()))
            .ReturnsAsync((TaskGetByIdResult.Success, new List<TaskItemOutput> { mockDbOutput }));

        var request = new InputQueryTaskDto { Id = taskId, UserId = userId };

        // Act
        var response = await _businessObject.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("Success", response.Status);
        Assert.Equal("Task retrieved successfully", response.Description);
        Assert.Single(response.Result);
        Assert.Equal(taskId, response.Result[0].Id);
    }
}