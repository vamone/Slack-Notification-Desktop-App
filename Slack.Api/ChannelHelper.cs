using System;
using System.Collections.Generic;

using Slack.Intelligence;

namespace Slack.Api
{
    public class ChannelHelper
    {
        public static ICollection<Channel> GetChannels(string token)
        {
            var url = RequestUrlFactory.BuildUrl(RequestUrlFactory.ChannelsListUrl, token);

            string json = WebRequestUtility.GetContent(url);

            var auth = GetChannelsInternal(json);

            return auth;
        }

        internal static ICollection<Channel> GetChannelsInternal(string json)
        {
            var content = JsonUtility.ConvertJsonIntoObject<ChannelParent>(json);
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            bool isStatusOk = content.IsStatusOk;
            if (!isStatusOk)
            {
                throw new SlackApiException(content.Error);
            }

            return content.Channels;
        }
    }
}