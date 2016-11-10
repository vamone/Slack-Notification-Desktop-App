using Newtonsoft.Json;

namespace Slack
{
    public class Message
    {
        [JsonProperty(PropertyName = "bot_id")]
        public string BotId { get; set; }

        public string BotName { get; set; }

        [JsonProperty(PropertyName = "user")]
        public string UserId { get; set; }

        public string UserName { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string MessageText { get; set; }

        public string ChannelName { get; set; }

        public bool IsGroupMessage { get; set; }

        public bool IsPrivateMessage { get; set; }

        [JsonProperty(PropertyName = "ts")]
        public double Timestamp { get; set; }

        internal bool IsValidated(Message message)
        {
            //TODO: EXPAND VALIDATION

            return (string.IsNullOrWhiteSpace(message.UserId) ||
                    string.IsNullOrWhiteSpace(message.BotId)) && string.IsNullOrWhiteSpace(message.MessageText) &&
                   string.IsNullOrWhiteSpace(message.ChannelName);
        }
    }
}