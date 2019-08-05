using Contracts;
using MediatR;

namespace Facade.API.Classes
{
    public class RequestMessage : Contract, IRequest<RequestResponse>
    {
    }
}
