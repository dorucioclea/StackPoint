using MediatR;

namespace Facade.API.Classes
{
    public interface IRequestMediatorService
    {
        void Request(RequestMessage message);
    }

    public class RequestMediatorService : IRequestMediatorService
    {

        private readonly IMediator _mediator;

        public RequestMediatorService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Request(RequestMessage message)
        {
            RequestResponse response = _mediator.Send(message).Result;
        }
    }
}
