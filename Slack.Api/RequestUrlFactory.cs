namespace Slack.Api
{
    public class RequestUrlFactory
    {
        private const string DomainName = "https://slack.com/api/";

        private const int TakeMessagesByCount = 10;

        public RequestUrlFactory(string token)
        {

            this.Auth = $"{DomainName}auth.test?token={token}&pretty=1";

            this.ChannelsList = $"{DomainName}channels.list?token={token}&pretty=1";

            this.ImsList = $"{DomainName}im.list?token={token}&pretty=1";

            this.UsersList = $"{DomainName}users.list?token={token}&pretty=1";

            this.EmojisList = $"{DomainName}emoji.list?token={token}&pretty=1";
        }

        public RequestUrlFactory(string token, string channelId) : this(token)
        {
            this.ChannelHistory =
                $"{DomainName}channels.history?token={token}&channel={channelId}&count={TakeMessagesByCount}&pretty=1";
            this.ImHistory =
                $"{DomainName}im.history?token={token}&channel={channelId}&count={TakeMessagesByCount}&pretty=1";
        }

        public RequestUrlFactory(string token, string channelId, string sendMessageText, string sendMessageAsUserId)
            : this(token)
        {
            this.SendMessage =
                $"{DomainName}chat.postMessage?token={token}&channel={channelId}&text={sendMessageText}&as_user={sendMessageAsUserId}&pretty=1";
        }

        public string Auth { get; protected set; }

        public string ChannelsList { get; protected set; }

        public string ChannelHistory { get; protected set; }

        public string ImsList { get; protected set; }

        public string ImHistory { get; protected set; }

        public string SendMessage { get; protected set; }

        public string UsersList { get; protected set; }

        public string EmojisList { get; protected set; }
    }
}