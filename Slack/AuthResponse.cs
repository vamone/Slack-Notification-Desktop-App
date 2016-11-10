using Newtonsoft.Json;

namespace Slack
{
    public class AuthResponse
    {
        [JsonProperty(PropertyName = "ok")]
        public bool IsStatusOk { get; set; }

        [JsonProperty(PropertyName = "team")]
        public string TeamName { get; set; }

        [JsonProperty(PropertyName = "user")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "team_id")]
        public string TeamId { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }
}