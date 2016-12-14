using System;

namespace Slack.Api
{
    public class AuthException : Exception
    {
        public AuthException(string message) : base(message)
        {
        }
    }
}