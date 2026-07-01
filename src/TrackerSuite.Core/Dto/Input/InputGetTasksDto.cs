using System.Text.Json.Serialization;
using MediatR;
using TrackerSuite.Core.Dto.Output;

namespace TrackerSuite.Core.Dto.Input;

public class InputGetTasksDto : IRequest<JsonResponseDto>
{
    [JsonIgnore]
    public Guid UserId { get; set; }
}
