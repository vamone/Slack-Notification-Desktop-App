using System;
using Slack.Intelligence;

namespace Slack.Desktop.Application
{
    public class MessageHelper
    {
        public static void AddMessage(Func<Message, Action> addMessageAction, string messageText, string userName = null,
            string channelName = null)
        {
            var message = new Message
            {
                MessageText = messageText,
                UserName = string.IsNullOrWhiteSpace(userName) ? "system" : userName,
                ChannelName = string.IsNullOrWhiteSpace(channelName) ? string.Empty : channelName,
                Timestamp = DateTimeUtility.DateTimeToUnixTimestamp(DateTime.Now)
            };

            addMessageAction.Invoke(message);
        }
    }
}