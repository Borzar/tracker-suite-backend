using MediatR;
using TrackerSuite.Core.Dto.Output;

namespace TrackerSuite.Core.Dto.Input;

public class InputDeleteTaskDto : IRequest<JsonResponseDto>
{
    public Guid Id { get; set; }
}
