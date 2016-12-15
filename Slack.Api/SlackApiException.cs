using System;

namespace Slack.Api
{
    public class SlackApiException : Exception
    {
        public SlackApiException(string message) : base(message)
        {
        }
    }
}