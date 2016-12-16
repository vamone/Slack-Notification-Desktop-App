using System;
using System.Collections.Generic;
using System.Linq;

namespace Slack.Api
{
    public class SlackApi : ISlackApi
    {
        public SlackApi()
        {
            this.LastMessageAt = DateTime.Now;
        }

        public Components Components { get; set; }

        private string Token { get; set; }

        private string ProfileId { get; set; }

        private DateTime LastMessageAt { get; set; }

        public IEnumerable<Message> GetMessages()
        {
            var messages = new List<Message>();

            foreach (var channel in this.Components.Channels)
            {
                var messagesInternal =
                    MessageHelper.GetMessages(this.Token, channel.ChannelId, true)
                        .Where(IsMessageVisible)
                        .Select(
                            x =>
                                MessageHelper.FormatMessage(x, this.Components, this.Token, null, channel.ChannelId,
                                    isPrivateMessage: false,
                                    isChannelMessage: true));
                messages.AddRange(messagesInternal);
            }

            foreach (var im in this.Components.Ims)
            {
                var messagesInternal = MessageHelper.GetMessages(this.Token, im.ImId)
                    .Where(this.IsMessageVisible)
                    .Select(
                        x =>
                            MessageHelper.FormatMessage(x, this.Components, this.Token, im.ImId, null,
                                isPrivateMessage: true));
                messages.AddRange(messagesInternal);
            }

            return messages;
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

                var components = new Components
                {
                    Profile = ProfileHelper.GetProfile(users, auth.UserId),
                    Channels = ChannelHelper.GetChannels(token),
                    Ims = ImHelper.GetIms(token),
                    Users = users,
                    Bots = BotsHelper.GetBots(token),
                    Emojis = new List<EmojiParent>()
                };

                this.Token = token;
                this.ProfileId = auth.UserId;
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
            //message.Validate();

            bool isSended = MessageHelper.IsMessageSended(message, this.Token);
            if (!isSended)
            {
                throw new SlackApiException("Could not send message.");
            }
        }

        internal bool IsMessageVisible(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (string.IsNullOrWhiteSpace(message.MessageText))
            {
                return false;
            }

            bool isMessageVisible = MessageHelper.IsMessageVisible(message, this.LastMessageAt, this.ProfileId);
            if (isMessageVisible)
            {
                this.LastMessageAt = DateTime.Now;

                return true;
            }

            return false;
        }
    }
}