namespace Slack.Notification.Service
{
    public class InitializationStatus
    {
        private const string DefaultInitializationMessage = "Initialization Successful.";

        public InitializationStatus()
        {
            this.Message = DefaultInitializationMessage;
        }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }
    }
}