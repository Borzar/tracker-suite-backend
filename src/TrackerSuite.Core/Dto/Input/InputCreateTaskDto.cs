using MediatR;
using TrackerSuite.Core.Dto.Output;

namespace TrackerSuite.Core.Dto.Input;

public class InputCreateTaskDto : IRequest<JsonResponseDto>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
}
