using System;
using System.Collections.Generic;
using System.Linq;

using Slack.Intelligence;

namespace Slack.Notification.Service
{
    public class MockSlackApiHelper : SlackApiHelper
    {
        public override ICollection<Message> GetMessages()
        {
            return MockMessages.GetMockMessages().ToList();
        } 

        internal override AuthResponse Auth(string token)
        {
            string json = JsonUtility.ReadJsonFromFile(fileName: "auth");
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentException(nameof(json));
            }

            var auth = this.ConvertJsonIntoContent<AuthResponse>(json);
            if (auth == null)
            {
                throw new ArgumentNullException(nameof(auth));
            }

            return auth;
        }

        internal override ICollection<Channel> GetChannels()
        {
            string json = JsonUtility.ReadJsonFromFile(fileName: "channels");

            var content = this.ConvertJsonIntoContent<ChannelParent>(json);

            bool isStatusOk = content.IsStatusOk;
            if (!isStatusOk)
            {
                throw new SlackApiHelpereException(content.Error);
            }

            return content.Channels;
        }

        protected override ICollection<Im> GetIms()
        {
            string json = JsonUtility.ReadJsonFromFile(fileName: "ims");

            var content = this.ConvertJsonIntoContent<ImParent>(json);

            bool isStatusOk = content.IsStatusOk;
            if (!isStatusOk)
            {
                throw new SlackApiHelpereException(content.Error);
            }

            return content.Ims;
        }

        protected override ICollection<User> GetUsers()
        {
            string json = JsonUtility.ReadJsonFromFile(fileName: "users");

            var content = this.ConvertJsonIntoContent<UserParent>(json);

            bool isStatusOk = content.IsStatusOk;
            if (!isStatusOk)
            {
                throw new SlackApiHelpereException(content.Error);
            }

            return content.Members;
        }
    }
}