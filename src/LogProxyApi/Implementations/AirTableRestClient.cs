using LogProxyApi.Abstractions;
using LogProxyApi.Dtos.AirTableApi;
using LogProxyApi.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LogProxyApi.Implementations
{
    public class AirTableRestClient : RestClient, IAirTableRestClient
    {
        private readonly AirTableApiOptions _apiOptions;

        public AirTableRestClient(IOptions<AirTableApiOptions> apiOptions)
        {
            _apiOptions = apiOptions?.Value ?? throw new ArgumentNullException(nameof(apiOptions));
            this.UseNewtonsoftJson(
                new JsonSerializerSettings() { 
                    ContractResolver = new DefaultContractResolver { 
                        NamingStrategy = new DefaultNamingStrategy { 
                            ProcessDictionaryKeys = false
                        }
                    },
                    NullValueHandling = NullValueHandling.Ignore
                });
        }

        private IRestRequest CreateRestRequestWithAuthorization(string resource, Method method) //TODO abstract this method to own interface/class
        {
            var req = new RestRequest(resource, method, DataFormat.Json);
            req.AddHeader("Authorization", $"Bearer {_apiOptions.ApiKey}");
            return req;
        }

        public async Task<IEnumerable<Record>> GetRecords(CancellationToken cancellationToken = default)
        {
            var request = CreateRestRequestWithAuthorization(_apiOptions.GetMessagesEndpoint, Method.GET);
            var response = await ExecuteGetAsync(request, cancellationToken);
            if (!response.IsSuccessful)
                return null;

            var jobj = JObject.Parse(response.Content);
            var records = jobj["records"].ToObject<Record[]>();

            return records;
        }

        public async Task<bool> TransferRecords(IEnumerable<Record> records, 
            CancellationToken cancellationToken = default)
        {
            var request = CreateRestRequestWithAuthorization(_apiOptions.PostMessagesEndpoint, Method.POST);
            request.AddJsonBody(new { records = records });

            var response = await ExecutePostAsync(request, cancellationToken);

            return response.IsSuccessful;
        }
    }
}
