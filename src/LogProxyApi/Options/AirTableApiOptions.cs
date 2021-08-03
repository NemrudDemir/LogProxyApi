using Flurl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogProxyApi.Options
{
    public class AirTableApiOptions
    {
        public string Host { get; set; }
        public string GetMessagesEndpoint => Url.Combine(Host, "Messages?maxRecords=3&view=Grid%20view");
        public string PostMessagesEndpoint => Url.Combine(Host, "Messages");
        public string ApiKey { get; set; }
    }
}
