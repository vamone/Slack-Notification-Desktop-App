using System;
using System.Threading;

using Slack.Intelligence;
using Slack.Notification.Service;

namespace Slack.Notification.Console.Application
{
    public class Program
    {
        static readonly Lazy<SlackApiHelper> SlackLazy = new Lazy<SlackApiHelper>();

        static SlackApiHelper Slack => SlackLazy.Value;

        public static void Main(string[] args)
        {
            try
            {
                string token = RegistryUtility.Read("MyToken");

                var conponents = Slack.Initialize(token);
                if (conponents == null)
                {
                    throw new ArgumentNullException(nameof(conponents));
                }

                System.Console.WriteLine(conponents.Result.Message);

                System.Console.WriteLine("Press enter to continuum.");
                System.Console.ReadLine();

                if (!conponents.Result.IsSuccess)
                {
                    return;
                }

                var thread = new Thread(GetMessagesInternal);
                thread.Start();

                var message = new Message
                {
                    ChannelId = "general",
                    MessageText = "Testar om meddelande kommer fram."
                };

                Slack.SendMessage(message);
            }
            catch (Exception ex)
            {
                ExceptionLogging.Trace(ex);

                System.Console.WriteLine(ex.Message);
            }
        }

        private static void GetMessagesInternal()
        {
            try
            {
                var messages = Slack.GetMessages();

                foreach (var message in messages)
                {
                    System.Console.WriteLine($"{message.UserName}#{message.ChannelName}: {message.MessageText}");
                }

                GetMessagesInternal();
            }
            catch (Exception ex)
            {
                ExceptionLogging.Trace(ex);

                System.Console.WriteLine(ex.Message);
            }
        }
    }
}