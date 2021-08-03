using LogProxyApi.Dtos.AirTableApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogProxyApi.Abstractions
{
    public interface IThirdPartyApiCommunicator
    {
        Task<IEnumerable<Message>> GetMessages();
        Task<bool> TransferMessage(Message message);
    }
}
