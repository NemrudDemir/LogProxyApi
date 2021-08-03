using AutoMapper;
using LogProxyApi.Abstractions;
using LogProxyApi.Dtos.AirTableApi;
using LogProxyApi.Dtos.Receiving;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogProxyApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/")]
    public class LogProxyController : ControllerBase
    {
        private readonly IThirdPartyApiCommunicator _apiCommunicator;
        private readonly IMapper _mapper;
        private readonly IIdGenerator _idGenerator;
        private readonly ILogger<LogProxyController> _logger;

        public LogProxyController(IThirdPartyApiCommunicator apiCommunicator, 
            IMapper mapper, IIdGenerator idGenerator, ILogger<LogProxyController> logger)
        {
            _apiCommunicator = apiCommunicator;
            _mapper = mapper;
            _idGenerator = idGenerator;
            _logger = logger;
        }

        [HttpPost("messages")]
        public async Task<IActionResult> Post([FromBody] PostMessage postMessage)
        {
            var message = _mapper.Map<Message>(postMessage);
            message.ReceivedAt = DateTime.Now;
            message.Id = _idGenerator.GetId();

            await _apiCommunicator.TransferMessage(message);
            _logger.LogInformation("Post was successful");
            return NoContent();
        }

        [HttpGet("messages")]
        public async Task<IEnumerable<Message>> Get()
        {
            var messages = await _apiCommunicator.GetMessages();
            _logger.LogInformation("Found {number} messages", messages.Count());
            return messages;
        }
    }
}
