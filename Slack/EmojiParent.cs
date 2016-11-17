using System.Collections.Generic;

using Newtonsoft.Json;

namespace Slack
{
    public class EmojiParent
    {
        [JsonProperty(PropertyName = "ok")]
        public bool IsStatusOk { get; set; }

        public string Error { get; set; }

        [JsonProperty(PropertyName = "emoji")]
        public ICollection<Dictionary<string, string>> Emojis { get; set; }

        [JsonProperty(PropertyName = "cache_ts")]
        public double CacheTimestamp { get; set; }
    }
}