using System.Collections.Generic;

namespace Slack.Api
{
    public interface ISlackApi
    {
        InitializationResults Initialize(string token);

        Components Components { get; set; }

        void SendMessage(Message message);

        IEnumerable<Message> GetMessages();
    }
}