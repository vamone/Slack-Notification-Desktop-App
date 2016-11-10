using Newtonsoft.Json;

namespace Slack
{
    public class Im
    {
        [JsonProperty(PropertyName = "id")]
        public string ImId { get; set; }

        [JsonProperty(PropertyName = "user")]
        public string UserId { get; set; }
    }
}