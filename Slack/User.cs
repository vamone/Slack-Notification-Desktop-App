using Newtonsoft.Json;

namespace Slack
{
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string UserName { get; set; }

        public string Error { get; set; }
    } 
}