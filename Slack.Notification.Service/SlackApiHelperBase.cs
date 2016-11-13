using System;
using System.Collections.Generic;
using System.Linq;

using Slack.Intelligence;

using Newtonsoft.Json;

namespace Slack.Notification.Service
{
    public class SlackApiHelperBase
    {
        protected DateTime LastMessageAt;

        protected string Token;

        protected string UserId;

        public SlackApiHelperBase()
        {
            this.Components = this.Components ?? new Components();

            this.LastMessageAt = DateTime.Now;
        }

        public Components Components { get; protected set; }

        public User MyPofile { get; protected set; }

        public ICollection<Message> GetMessages() //TODO: MAKE IT GENRIC TYPE
        {
            var messages = new List<Message>();

            foreach (var channel in this.Components.Channels)
            {
                var url = string.Format(RequestConfig.ChannelsHistoryUrl, this.Token, channel.ChannelId,
                    RequestConfig.TakeMessageByRequest);

                var json = this.GetContent(url);

                var messagesInternal = this.GetMessagesInternal(json, null, channel.ChannelId);
                messages.AddRange(messagesInternal);
            }

            foreach (var im in this.Components.Ims)
            {
                var url = string.Format(RequestConfig.ImHistoryUrl, this.Token, im.ImId,
                    RequestConfig.TakeMessageByRequest);

                var json = this.GetContent(url);

                var messagesInternal = this.GetMessagesInternal(json, im.ImId, null, true);

                messages.AddRange(messagesInternal);
            }

            return messages;
        }

        internal T ConvertJsonIntoContent<T>(string json)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentException(nameof(json));
            }

            var content = JsonConvert.DeserializeObject<T>(json);
            if (content == null)
            {
                throw new SlackApiHelpereException(nameof(content));
            }

            return content;
        }

        internal Message FormatMessage(Message message, string imId = null, string channelId = null,
            bool isPrivateMessage = false,
            bool isGroupMessage = false)
        {
            string userName = UserMethods.GetUserName(this.Components.Users, message.UserId);
            if (userName == null)
            {
                this.Components.Users = GetUsers();

                userName = UserMethods.GetUserName(this.Components.Users, message.UserId);
            }

            string botName = BotMethods.GetBotName(this.Components.Bots, message.BotId);
            if (botName == null)
            {
                this.Components.Bots = GetBots();

                botName = BotMethods.GetBotName(this.Components.Bots, message.BotId);
            }

            string imName = ImMethos.GetImName(this.Components.Ims, this.Components.Users, imId);
            if (imName == null)
            {
                this.Components.Ims = GetIms();

                imName = ImMethos.GetImName(this.Components.Ims, this.Components.Users, imId);
            }

            string channelName = ChannelMethods.GetChannelName(this.Components.Channels, channelId);
            if (channelName == null)
            {
                this.Components.Channels = GetChannels();

                channelName = ChannelMethods.GetChannelName(this.Components.Channels, channelId);
            }

            message.UserName = botName ?? userName;
            message.ChannelName = isPrivateMessage ? null : (imName ?? channelName);

            message.IsPrivateMessage = isPrivateMessage;
            message.IsGroupMessage = isGroupMessage;

            return message;
        }

        internal ICollection<Bot> GetBots()
        {
            return new List<Bot>(); //TODO: READ FROM EXTERNAL JSON FILE
        }

        internal ICollection<Channel> GetChannels()
        {
            var url = string.Format(RequestConfig.ChannelsListUrl, this.Token);
            var json = this.GetContent(url);

            var content = this.ConvertJsonIntoContent<ChannelParent>(json);

            bool isStatusOk = content.IsStatusOk;
            if (!isStatusOk)
            {
                throw new SlackApiHelpereException(content.Error);
            }

            return content.Channels;
        }

        internal virtual string GetContent(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException(nameof(url));
            }

            var content = WebRequestUtility.GetContent(url);
            if (content == null)
            {
                throw new SlackApiHelpereException(nameof(content));
            }

            return content;
        }

        internal ICollection<Emoji> GetEmojis()
        {
            return new List<Emoji>();
        }

        internal ICollection<Im> GetIms()
        {
            var url = string.Format(RequestConfig.ImListUrl, this.Token);

            var json = this.GetContent(url);

            var content = this.ConvertJsonIntoContent<ImParent>(json);

            bool isStatusOk = content.IsStatusOk;
            if (!isStatusOk)
            {
                throw new SlackApiHelpereException(content.Error);
            }

            return content.Ims;
        }

        internal ICollection<Message> GetMessagesInternal(string json, string imId = null, string channelId = null,
            bool isPrivateMessage = false,
            bool isGroupMessage = false)
        {
            var data = this.ConvertJsonIntoContent<MessageParent>(json);

            bool isStatusOk = data.IsStatusOk;
            if (!isStatusOk)
            {
                throw new SlackApiHelpereException(data.Error);
            }

            return
                data.Messages.Where(IsMessageVisible)
                    .Select(x => FormatMessage(x, imId, channelId, isPrivateMessage, isGroupMessage))
                    .ToList();
        }

        internal ICollection<User> GetUsers()
        {
            var url = string.Format(RequestConfig.UserListUrl, this.Token);
            var json = this.GetContent(url);

            var content = this.ConvertJsonIntoContent<UserParent>(json);

            bool isStatusOk = content.IsStatusOk;
            if (!isStatusOk)
            {
                throw new SlackApiHelpereException(content.Error);
            }

            return content.Members;
        }

        internal bool IsMessageVisible(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            //bool isMessageValidated = message.IsValidated(message);
            //if (!isMessageValidated)
            //{
            //    throw new SlackApiHelpereException(nameof(isMessageValidated));
            //}

            if (string.IsNullOrWhiteSpace(message?.MessageText))
            {
                return false;
            }

            var messageInDateTime = DateTimeUtility.UnixTimeStampToDateTime(message.Timestamp);

            bool isMessageNew = this.LastMessageAt < messageInDateTime;
            bool isOwnMessage = message.UserId == this.UserId;

            bool isMessageVisible = isMessageNew && !isOwnMessage;
            if (isMessageVisible)
            {
                this.LastMessageAt = messageInDateTime;

                return true;
            }

            return false;
        }
    }
}