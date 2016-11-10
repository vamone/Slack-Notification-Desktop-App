using System.Collections.Generic;

using Newtonsoft.Json;

namespace Slack
{
    public class UserParent
    {
        [JsonProperty(PropertyName = "ok")]
        public bool IsStatusOk { get; set; }

        public string Error { get; set; }

        [JsonProperty(PropertyName = "members")]
        public ICollection<User> Members { get; set; }

        [JsonProperty(PropertyName = "cache_ts")]
        public double CacheTimestamp { get; set; }
    }
}