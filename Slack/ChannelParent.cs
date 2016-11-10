using System.Collections.Generic;

using Newtonsoft.Json;

namespace Slack
{
    public class ChannelParent
    {
        [JsonProperty(PropertyName = "ok")]
        public bool IsStatusOk { get; set; }

        [JsonProperty(PropertyName = "channels")]
        public ICollection<Channel> Channels { get; set; }

        public string Error { get; set; }
    }
}