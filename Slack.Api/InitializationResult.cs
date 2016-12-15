using System.Diagnostics;

namespace Slack.Api
{
    [DebuggerDisplay("IsSuccess={IsSuccess}, Message={Message}")]
    public class InitializationResults
    {
        private const string DefaultInitializationMessage = "Initialization Successful.";

        public InitializationResults()
        {
            this.Message = DefaultInitializationMessage;
        }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }
    }
}