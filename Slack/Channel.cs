using Newtonsoft.Json;

namespace Slack
{
    public class Channel
    {
        [JsonProperty(PropertyName = "id")]
        public string ChannelId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string ChannelName { get; set; }
    }
}