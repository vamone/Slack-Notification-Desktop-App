using System;
using System.Collections.Generic;

using Slack.Intelligence;

namespace Slack.Api
{
    public class MockSlackApi : ISlackApi
    {
        public MockSlackApi()
        {
            this.IsMock = true;
        }

        public Components Components { get; set; }

        public bool IsMock { get; set; }

        public IEnumerable<Message> GetMessages()
        {
            return MockMessages.GetMockMessages();
        }

        public InitializationResults Initialize(string token)
        {
            var result = new InitializationResults();

            try
            {
                if (token == null)
                {
                    throw new SlackApiException(nameof(token));
                }

                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new SlackApiException(nameof(token));
                }

                //TODO: WHEN NO JSON FILES EXISTS, WHAT TO DO?

                string authJson = JsonUtility.ReadJsonFromFile(fileName: "auth");
                string usersJson = JsonUtility.ReadJsonFromFile(fileName: "users");
                string channelsJson = JsonUtility.ReadJsonFromFile(fileName: "channels");
                string imsJson = JsonUtility.ReadJsonFromFile(fileName: "ims");

                var auth = AuthHelper.GetAuthResponseFromJson(authJson);
                var users = UserHelper.GetUsersFromJson(usersJson);

                var components = new Components
                {
                    Profile = ProfileHelper.GetProfile(users, auth.UserId),
                    Channels = ChannelHelper.GetChannelsFromJson(channelsJson),
                    Ims = ImHelper.GetImsFromJson(imsJson),
                    Users = users,
                    Bots = BotsHelper.GetBotsFromJson(),
                    Emojis = new List<EmojiParent>()
                };

                this.Components = components;

                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public void SendMessage(Message message)
        {
            message.Validate();
        }
    }
}