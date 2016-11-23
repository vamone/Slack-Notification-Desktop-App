using Newtonsoft.Json;

namespace Slack
{
    public class MessageSendResponse
    {
        [JsonProperty(PropertyName = "ok")]
        public bool IsStatusOk { get; set; }

        public string Error { get; set; }

        [JsonProperty(PropertyName = "channel")]
        public string ChannelImGroupId { get; set; }

        [JsonProperty(PropertyName = "ts")]
        public double Timestamp { get; set; }

        [JsonProperty(PropertyName = "message")]
        public Message Message { get; set; }
    }
}