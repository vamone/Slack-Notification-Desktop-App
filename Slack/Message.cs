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

        public string ChannelId { get; set; }

        public bool IsGroupMessage { get; set; }

        public bool IsPrivateMessage { get; set; }

        [JsonProperty(PropertyName = "ts")]
        public double Timestamp { get; set; }

        public void Validate()
        {
            bool isValidated = this.IsValidated();
            if (!isValidated)
            {
                throw new ObjectNotValidatedException(typeof(Message));
            }
        }

        internal bool IsValidated()
        {
            return (string.IsNullOrWhiteSpace(this.UserId) ||
                    string.IsNullOrWhiteSpace(this.BotId)) && string.IsNullOrWhiteSpace(this.MessageText) &&
                   string.IsNullOrWhiteSpace(this.ChannelId);
        }
    }
}