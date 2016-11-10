using System;
using System.Collections.Generic;

using Slack.Intelligence;
using Slack.Notification.Service;

namespace Slack.Notification.Console.Application
{
    public class Program
    {
        private static readonly SlackApiHelper Slack = new SlackApiHelper();

        public static void Main(string[] args)
        {
            string token = RegistryUtility.Read("MyToken");

            var conponents = Slack.InitializeComponents(token);

            System.Console.WriteLine(conponents.Result.Message);

            try
            {
                var messages = GetMessagesInternal(Slack);
                foreach (var message in messages)
                {
                    System.Console.WriteLine($"{message.UserName}#{message.ChannelName}: {message.MessageText}");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            System.Console.ReadLine();
        }

        internal static IEnumerable<Message> GetMessagesInternal(SlackApiHelper api)
        {
            var messages = Slack.GetMessages();

            foreach (var message in messages)
            {
                yield return message;
            }

            var nextMessages = GetMessagesInternal(api);

            foreach (var message in nextMessages)
            {
                yield return message;
            }
        }
    }
}