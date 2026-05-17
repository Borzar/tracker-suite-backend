using MediatR;
using Models.Output;

namespace Models.Input;

public class InputCreateTask : IRequest<JsonResponse>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid UserId { get; set; }
}
