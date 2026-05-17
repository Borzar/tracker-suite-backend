using MediatR;
using Models.Output;

namespace Models.Input;

public class InputDeleteTask : IRequest<JsonResponse>
{
    public Guid Id { get; set; }
}
