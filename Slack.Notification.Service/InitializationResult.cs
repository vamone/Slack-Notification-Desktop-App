namespace Slack.Notification.Service
{
    public class InitializationResult
    {
       
        public InitializationResult()
        {
            this.Result = this.Result ?? new InitializationStatus();

            this.ResponseError = this.ResponseError ?? new AuthResponseError();

            this.Components = this.Components ?? new InitializationComponents();
        }

        public InitializationStatus Result { get; set; }

        public AuthResponseError ResponseError { get; set; }

        public InitializationComponents Components { get; set; }
    }
}