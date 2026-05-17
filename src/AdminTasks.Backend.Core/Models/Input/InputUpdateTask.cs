using MediatR;
using Models.Output;

namespace Models.Input;

public class InputUpdateTask : InputCreateTask, IRequest<JsonResponse>
{
    public Guid Id { get; set; }
}
