using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slack.Notification.Service
{
    public class SlackApi : ISlackApi
    {
        public SlackApi()
        {
            this.MyProfile = this.MyProfile ?? new User();
            this.Compontents = this.Compontents ?? new Components();
        }

        public InitializationResult InitializeComponents(string token)
        {
            throw new NotImplementedException();
        }

        public User MyProfile { get; set; }

        public Components Compontents { get; set; }

        public void SendMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Message> GetMessages()
        {
            throw new NotImplementedException();
        }
    }
}