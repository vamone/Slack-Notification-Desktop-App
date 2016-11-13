using System;
using System.Diagnostics;

namespace Slack.Intelligence
{
    [DebuggerDisplay("IsSuccess = {IsSuccess}, ErrorMessage = {ErrorMessage}")]
    public class WebRequestResult
    {
        public string Content { get; set; }

        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }

        public Exception Exception { get; set; }
    }
}