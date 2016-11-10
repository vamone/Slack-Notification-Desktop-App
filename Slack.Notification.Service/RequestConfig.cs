namespace Slack.Notification.Service
{
    internal class RequestConfig
    {
        internal const string ChannelsListUrl = "https://slack.com/api/channels.list?token={0}&pretty=1";

        internal const string ChannelsHistoryUrl =
            "https://slack.com/api/channels.history?token={0}&channel={1}&count={2}&pretty=1";

        internal const string ImHistoryUrl = "https://slack.com/api/im.history?token={0}&channel={1}&count={2}&pretty=1";

        internal const string ImListUrl = "https://slack.com/api/im.list?token={0}&pretty=1";

        internal const string PostMessageUrl =
            "https://slack.com/api/chat.postMessage?token={0}&channel={1}&text={2}&as_user={3}&pretty=1";

        internal const int TakeMessageByRequest = 10;

        internal const string UserListUrl = "https://slack.com/api/users.list?token={0}&pretty=1";

        internal const string AuthTestUrl = "https://slack.com/api/auth.test?token={0}&pretty=1";
    }
}