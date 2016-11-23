using System.Collections.Generic;

namespace Slack.Notification.Service
{
    public interface ISlackApi
    {
        InitializationResult InitializeComponents(string token);

        User MyProfile { get; set; }

        Components Compontents { get; set; }

        void SendMessage(Message message);

        IEnumerable<Message> GetMessages();
    }
}