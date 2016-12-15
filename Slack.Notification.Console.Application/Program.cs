using System;
using System.Threading;

using Slack.Api;
using Slack.Intelligence;

namespace Slack.Notification.Console.Application
{
    public class Program
    {
        static readonly Lazy<SlackApi> SlackLazy = new Lazy<SlackApi>();

        static SlackApi Slack => SlackLazy.Value;

        public static void Main(string[] args)
        {
            try
            {
                string token = RegistryUtility.Read("MyToken");

                var init = Slack.Initialize(token);
                if (init == null)
                {
                    throw new ArgumentNullException(nameof(init));
                }

                System.Console.WriteLine(init.Message);

                if (!init.IsSuccess)
                {
                    return;
                }

                var thread = new Thread(GetMessages);
                thread.Start();

                //var message = new Message
                //{
                //    ChannelId = "general",
                //    MessageText = "Testar om meddelande kommer fram."
                //};

                //Slack.SendMessage(message);
            }
            catch (Exception ex)
            {
                ExceptionLogging.Trace(ex);

                System.Console.WriteLine(ex.Message);

                System.Console.ReadLine();
            }
        }

        static void GetMessages()
        {
            bool hasException = false;

            while (true)
            {
                try
                {
                    var messages = Slack.GetMessages();

                    foreach (var message in messages)
                    {
                        System.Console.WriteLine($@"{message.UserName}#{message.ChannelName}: {message.MessageText}");
                    }
                }
                catch (Exception ex)
                {
                    if (!hasException)
                    {
                        ExceptionLogging.Trace(ex);

                        System.Console.WriteLine(ex.Message);

                        hasException = true;
                    }
                }
            }
        }
    }
}