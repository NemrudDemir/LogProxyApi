using System;

namespace LogProxyApi.Dtos.AirTableApi
{
    public class Message
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime? ReceivedAt { get; set; }
    }
}
