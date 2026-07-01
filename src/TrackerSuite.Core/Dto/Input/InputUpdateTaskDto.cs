using System.Text.Json.Serialization;
using MediatR;
using TrackerSuite.Core.Dto.Output;

namespace TrackerSuite.Core.Dto.Input;

public class InputUpdateTaskDto : IRequest<JsonResponseDto>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    [JsonIgnore]
    public Guid Id { get; set; }
    [JsonIgnore]
    public Guid UserId { get; set; }
}
