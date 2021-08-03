using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LogProxyApi.Dtos.AirTableApi
{
    public class Record
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("createdtime")]
        public DateTime? CreatedTime { get; set; }
        [JsonProperty("fields")]
        public Dictionary<string, object> Fields { get; set; } = new Dictionary<string, object>();
    }
}
