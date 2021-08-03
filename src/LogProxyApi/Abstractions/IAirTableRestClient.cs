using LogProxyApi.Dtos.AirTableApi;
using RestSharp;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LogProxyApi.Abstractions
{
    public interface IAirTableRestClient : IRestClient
    {
        Task<IEnumerable<Record>> GetRecords(CancellationToken cancellationToken = default);
        Task<bool> TransferRecords(IEnumerable<Record> records, CancellationToken cancellationToken = default);
    }
}
