using MediatR;
using TrackerSuite.Core.Dto.Output;

namespace TrackerSuite.Core.Dto.Input;

public class InputQueryTaskDto : IRequest<JsonResponseDto>
{
    public Guid Id { get; set; }
}
