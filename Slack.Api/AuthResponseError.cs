namespace Slack
{
    public class AuthResponseError
    {
        public string ErrorCode { get; set; }

        public string Description { get; set; }

        public string SolutionMessage { get; set; }

        public bool IsWarning { get; set; }
    }
}