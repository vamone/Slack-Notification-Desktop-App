using System;
using System.Collections.Generic;

namespace Slack.Api
{
    public class SlackApi : ISlackApi
    {
        public Components Components { get; set; }

        public IEnumerable<Message> GetMessages()
        {
            throw new NotImplementedException();
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

                var auth = AuthHelper.GetAuthResponse(token);

                var users = UserHelper.GetUsers(token);

                this.Components.Profile = ProfileHelper.GetProfile(users, auth.UserId);
                this.Components.Channels = ChannelHelper.GetChannels(token);
                this.Components.Ims = ImHelper.GetIms(token);
                this.Components.Users = users;
                this.Components.Bots = BotsHelper.GetBots(token);
                this.Components.Emojis = new List<EmojiParent>();

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