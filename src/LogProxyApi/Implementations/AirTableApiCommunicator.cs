using AutoMapper;
using LogProxyApi.Abstractions;
using LogProxyApi.Dtos.AirTableApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogProxyApi.Implementations
{
    public class AirTableApiCommunicator : IThirdPartyApiCommunicator
    {
        private readonly IAirTableRestClient _restClient;
        private readonly IMapper _mapper;

        public AirTableApiCommunicator(IAirTableRestClient restClient, IMapper mapper)
        {
            _restClient = restClient;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Message>> GetMessages()
        {
            var records = await _restClient.GetRecords();
            var messages = _mapper.Map<IEnumerable<Message>>(records);

            return messages;
        }

        public Task<bool> TransferMessage(Message message)
        {
            return TransferMessages(new[] { message });
        }

        private async Task<bool> TransferMessages(IEnumerable<Message> messages)
        {
            var records = _mapper.Map<IEnumerable<Record>>(messages);
            return await _restClient.TransferRecords(records);
        }
    }
}
