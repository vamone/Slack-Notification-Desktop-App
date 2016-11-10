using System;
using System.Collections.Generic;
using System.Linq;

namespace Slack.Intelligence
{
    public static class ChannelMethods
    {
        public static string GetChannelName(ICollection<Channel> channels, string channelId)
        {
            if (channels == null)
            {
                throw new ArgumentNullException(nameof(channels));
            }

            var channel = channels.FirstOrDefault(x => x.ChannelId == channelId);

            return channel?.ChannelName;
        }
    }
}