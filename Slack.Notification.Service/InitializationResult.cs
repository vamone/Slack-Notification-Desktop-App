namespace Slack.Notification.Service
{
    public class InitializationResult
    {
        public InitializationResult()
        {
            this.Result = this.Result ?? new InitializationStatus();
            this.ResponseError = this.ResponseError ?? new AuthResponseError();
        }

        public InitializationStatus Result { get; set; }

        public AuthResponseError ResponseError { get; set; }
    }
}