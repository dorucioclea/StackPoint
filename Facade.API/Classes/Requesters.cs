using Facade.API.Hubs;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace Facade.API.Classes
{
    public class Requester : IRequestHandler<RequestMessage, RequestResponse>
    {
        private readonly IRequestClient<RequestMessage, RequestResponse> _requestClient;
        private readonly IHubContext<ResponseHub> _hubContext;
        public Requester(IRequestClient<RequestMessage, RequestResponse> requestClient, IHubContext<ResponseHub> hubContext)
        {
            _requestClient = requestClient;
            _hubContext = hubContext;
        }
        public async Task<RequestResponse> Handle(RequestMessage message, CancellationToken cancellationToken)
        {
            RequestResponse result = null;
            try
            {
                result = await _requestClient.Request(message);
            }
            catch (RequestTimeoutException)
            {
                result = new RequestResponse { Data = false };
            }
            await _hubContext.Clients.All.SendAsync("Receive", $"Message: |Contract:{message.ContractId}" +
                                                             $"|CreationDate:{message.CreationDate}" +
                                                             $"|Number:{message.Number}" +
                                                             $"|Price:{message.Price}" +
                                                             $"|Consumer:{message.ConsumerId}" +
                                                             $"|Contractor:{message.ContractorId}|" +
                                                             $" SendState: {result.Data}");
            return result;
        }
    }

}
