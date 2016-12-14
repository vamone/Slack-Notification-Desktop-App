using System;

namespace Slack.Api
{
    public class SlackApiException : Exception
    {
        private const string ExceptionFormat = "Slack API threw an exception: {0}, at: {1}.";

        public SlackApiException(string message) : base(FormatException(message))
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