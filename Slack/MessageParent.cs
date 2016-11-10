using System.Collections.Generic;

using Newtonsoft.Json;

namespace Slack
{
    public class MessageParent
    {
        [JsonProperty(PropertyName = "ok")]
        public bool IsStatusOk { get; set; }

        public string Error { get; set; }

        public ICollection<Message> Messages { get; set; }

        [JsonProperty(PropertyName = "has_more")]
        public bool HasMore { get; set; }
    }
}