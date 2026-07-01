using System.Text.Json.Serialization;
using MediatR;
using TrackerSuite.Core.Dto.Output;

namespace TrackerSuite.Core.Dto.Input;

public class InputQueryTaskDto : IRequest<JsonResponseDto>
{
    public Guid Id { get; set; }
    [JsonIgnore]
    public Guid UserId { get; set; }
}
