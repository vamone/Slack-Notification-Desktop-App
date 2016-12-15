using System;
using System.Collections.Generic;

using Slack.Intelligence;

namespace Slack.Api
{
    public static class ChannelHelper
    {
        public static ICollection<Channel> GetChannels(string token)
        {
            var url = new RequestUrlFactory(token);

            string json = WebRequestUtility.GetContent(url.ChannelsList);

            var auth = GetChannelsFromJson(json);

            return auth;
        }

        public static ICollection<Channel> GetChannelsFromJson(string json)
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