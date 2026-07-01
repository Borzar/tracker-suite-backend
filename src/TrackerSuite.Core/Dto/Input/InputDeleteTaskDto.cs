using System.Text.Json.Serialization;
using MediatR;
using TrackerSuite.Core.Dto.Output;

namespace TrackerSuite.Core.Dto.Input;

public class InputDeleteTaskDto : IRequest<JsonResponseDto>
{
    public Guid Id { get; set; }
    [JsonIgnore]
    public Guid UserId { get; set; }
}
