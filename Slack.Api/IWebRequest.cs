namespace Slack.Api
{
    public interface IWebRequest
    {
        string GetContent(string url);
    }
}