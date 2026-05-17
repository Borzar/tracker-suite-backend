using MediatR;
using Models.Output;

namespace Models.Input;

public class InputQueryTask : IRequest<JsonResponse>
{
    public Guid Id { get; set; }
}
