using System;
using System.Collections.Generic;

using Slack;
using Slack.Intelligence;

namespace SlackDesktopBubbleApplication
{
    public static class MockMessages
    {
        public static ICollection<Message> GetMockMessages()
        {
            var messages = new List<Message>
            {
                new Message
                {
                    MessageText = "Idag är måndag och det är dags att focusera.",
                    UserName = "Testare",
                    ChannelName = "Random",
                    Timestamp = DateTimeUtility.DateTimeToUnixTimestamp(DateTime.Now)
                },
                new Message
                {
                    MessageText = "Ibland det kan vara svårt med vissa saker.",
                    UserName = "Testare",
                    ChannelName = "Random",
                    Timestamp = DateTimeUtility.DateTimeToUnixTimestamp(DateTime.Now)
                }
            };

            return messages;
        }
    }
}