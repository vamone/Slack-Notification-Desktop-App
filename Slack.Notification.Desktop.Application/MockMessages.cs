using System;
using System.Collections.Generic;
using System.Threading;

using Slack;
using Slack.Intelligence;

namespace SlackDesktopBubbleApplication
{
    public static class MockMessages
    {
        public static IEnumerable<Message> GetMockMessages()
        {
            Thread.Sleep(3000);

            yield return new Message
            {
                MessageText = "You joining our BA call at the OG office?",
                UserName = "Marcus Balt",
                IsPrivateMessage = true,
                Timestamp = DateTimeUtility.DateTimeToUnixTimestamp(DateTime.Now.AddSeconds(5))
            };

            Thread.Sleep(5000);

            yield return new Message
            {
                MessageText = "What about lunch today?",
                UserName = "Andrea Lynnblad",
                ChannelName = "Random",
                Timestamp = DateTimeUtility.DateTimeToUnixTimestamp(DateTime.Now.AddSeconds(3))
            };

            Thread.Sleep(3000);

            yield return new Message
            {
                MessageText = "Yes, we go about an hour ;)",
                UserName = "Patrik Ström",
                ChannelName = "Random",
                Timestamp = DateTimeUtility.DateTimeToUnixTimestamp(DateTime.Now.AddSeconds(8))
            };

            Thread.Sleep(8000);

            yield return new Message
            {
                MessageText =
                    "Free Agile training resources from Coursera. I highly recommend this site! I have taken almost dozen courses in past three years- videos always engaging and easy to follow. Each section comes with short quizzes, video transcripts and notes to facilitate learning. I have more helpful info so feel free to drop me a line!",
                UserName = "Name name",
                ChannelName = "Random",
                Timestamp = DateTimeUtility.DateTimeToUnixTimestamp(DateTime.Now.AddSeconds(5))
            };

            yield return new Message
            {
                MessageText = "Interesting approach by our competitor to innovative. Interesting to see more of their work.",
                UserName = "Name2 name2",
                ChannelName = "Random",
                Timestamp = DateTimeUtility.DateTimeToUnixTimestamp(DateTime.Now.AddSeconds(2))
            };

            yield return new Message
            {
                MessageText = "Thanks Mike! Great to hear that the Team found value in getting that deeper business understanding.",
                UserName = "Name3 name3",
                ChannelName = "Random",
                Timestamp = DateTimeUtility.DateTimeToUnixTimestamp(DateTime.Now.AddSeconds(2))
            };

            yield return new Message
            {
                MessageText =
                    "Got to see ESRI in action on Prysm today! Super exited- This one of several technologies we're researching fir digital collaboration and geospatial visualisation. More to come soon but let me know how rest of you found todays session?",
                UserName = "Name4 Name4",
                ChannelName = "General",
                Timestamp = DateTimeUtility.DateTimeToUnixTimestamp(DateTime.Now.AddSeconds(3))
            };

            yield return new Message
            {
                MessageText = "YI find our sector super fascinating. The few big beats are all trying to find their way to the digital future! I appreciate you meerkat skills Donna :slightly_smiling_face: Can you check if Mark is still in the office please?",
                UserName = "Name5 Name5",
                ChannelName = "General",
                Timestamp = DateTimeUtility.DateTimeToUnixTimestamp(DateTime.Now.AddSeconds(5))
            };

            var messages = GetMockMessages();
            foreach (var message in messages)
            {
                yield return message;
            }
        }
    }
}