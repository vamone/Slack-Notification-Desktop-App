namespace Slack.Api
{
    public class RequestUrlFactory
    {
        private const string DomainName = "https://slack.com/api/";

        public static string BuildUrl(string token, string url)
        {
            return string.Format(url, token);
        }

        public static string ChannelsListUrl = $"{DomainName}channels.list?token={0}&pretty=1";

        public static string ChannelsHistoryUrl =
            $"{DomainName}channels.history?token={0}&channel={1}&count={2}&pretty=1";

        public static string ImHistoryUrl = $"{DomainName}im.history?token={0}&channel={1}&count={2}&pretty=1";

        public static string ImListUrl = $"{DomainName}im.list?token={0}&pretty=1";

        public static string PostMessageUrl =
            $"{DomainName}chat.postMessage?token={0}&channel={1}&text={2}&as_user={3}&pretty=1";

        public static int TakeMessageByRequest = 10;

        public static string UserListUrl = $"{DomainName}users.list?token={0}&pretty=1";

        public static string AuthTestUrl = $"{DomainName}auth.test?token={0}&pretty=1";

        public static string SendMessageUrl =
            $"{DomainName}chat.postMessage?token={0}&channel={1}&text={2}&as_user={3}&pretty=1";

        public static string EmojiUrl = $"{DomainName}emoji.list?token={0}&pretty=1";
    }
}