using System;

namespace Slack.Notification.Service
{
    public class SlackApiHelpereException : Exception
    {
        private const string ExceptionFormat = "SlackNotificationService threw an exception: {0}, at: {1}.";

        public SlackApiHelpereException(string message) : base(FormatException(message))
        {         
        }

        private static string FormatException(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return string.Empty;
            }

            return string.Format(ExceptionFormat, message, DateTime.Now);
        }
    }
}