using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TrackerSuite.Backend.Controllers;
using TrackerSuite.Core.Dto.Input;
using TrackerSuite.Core.Dto.Output;
using Xunit;

namespace TrackerSuite.Tests.Controllers;

public class TaskItemControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<TaskItemController>> _loggerMock;
    private readonly TaskItemController _controller;
    private readonly Guid _mockUserId;

    public TaskItemControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<TaskItemController>>();
        _controller = new TaskItemController(_loggerMock.Object, _mediatorMock.Object);
        _mockUserId = Guid.NewGuid();

        // Simular el usuario autenticado en el HttpContext (User.FindFirst)
        var claims = new List<Claim>
        {
            new Claim("sub", _mockUserId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };
    }

    [Fact]
    public async Task Create_ValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new InputCreateTaskDto { Title = "Unit Testing" };
        var expectedResponse = new JsonResponseDto 
        { 
            Status = "Success", 
            Description = "Task created successfully" 
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<InputCreateTaskDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Create(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var responseBody = Assert.IsType<JsonResponseDto>(okResult.Value);
        Assert.Equal("Success", responseBody.Status);
        Assert.Equal("Task created successfully", responseBody.Description);
    }

    [Fact]
    public async Task GetById_TaskDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var expectedResponse = new JsonResponseDto 
        { 
            Status = "NotFound", 
            Description = $"The task with ID {taskId} does not exist" 
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<InputQueryTaskDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(taskId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        var responseBody = Assert.IsType<JsonResponseDto>(notFoundResult.Value);
        Assert.Equal("NotFound", responseBody.Status);
    }

    [Fact]
    public async Task GetAll_UserHasTasks_ReturnsAllTasks()
    {
        // Arrange
        var expectedResponse = new JsonResponseDto 
        { 
            Status = "Success", 
            Description = "Tasks fetched successfully",
            Result = new List<TaskItemOutputDto> 
            { 
                new TaskItemOutputDto { Id = Guid.NewGuid(), Title = "Task 1" } 
            }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<InputGetTasksDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var responseBody = Assert.IsType<JsonResponseDto>(okResult.Value);
        Assert.Equal("Success", responseBody.Status);
        Assert.Single(responseBody.Result);
    }

    [Fact]
    public async Task Update_ValidRequest_ReturnsOk()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var request = new InputUpdateTaskDto { Title = "Updated Title" };
        var expectedResponse = new JsonResponseDto 
        { 
            Status = "Success", 
            Description = "Task updated successfully" 
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<InputUpdateTaskDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Update(taskId, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var responseBody = Assert.IsType<JsonResponseDto>(okResult.Value);
        Assert.Equal("Success", responseBody.Status);
    }

    [Fact]
    public async Task Delete_ExistingTask_ReturnsOk()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var expectedResponse = new JsonResponseDto 
        { 
            Status = "Success", 
            Description = "Task deleted successfully" 
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<InputDeleteTaskDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Delete(taskId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var responseBody = Assert.IsType<JsonResponseDto>(okResult.Value);
        Assert.Equal("Success", responseBody.Status);
    }
}