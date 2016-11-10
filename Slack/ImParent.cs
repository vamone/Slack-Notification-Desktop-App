using System.Collections.Generic;

using Newtonsoft.Json;

namespace Slack
{
    public class ImParent
    {
        [JsonProperty(PropertyName = "ok")]
        public bool IsStatusOk { get; set; }

        [JsonProperty(PropertyName = "ims")]
        public ICollection<Im> Ims { get; set; }

        public string Error { get; set; }
    }
}