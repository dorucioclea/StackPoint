using Contracts;
using Facade.API.Classes;
using Microsoft.AspNetCore.Mvc;

namespace Facade.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AddController : ControllerBase
    {
        private readonly IRequestMediatorService _notifierMediatorService;
        public AddController(IRequestMediatorService notifierMediatorService)
        {
            _notifierMediatorService = notifierMediatorService;
        }

        [HttpPost]
        public void Post([FromBody] Contract request)
        {
            RequestMessage contract = new RequestMessage
            {
                ConsumerId = request.ConsumerId,
                ContractId = request.ContractId,
                CreationDate = request.CreationDate,
                ContractorId = request.ContractorId,
                Number = request.Number,
                Price = request.Price
            };
            _notifierMediatorService.Request(contract);
        }
    }
}
