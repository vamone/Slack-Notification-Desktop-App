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

        public virtual void SendMessage(Message message)
        {
            string url = string.Format(RequestUrls.SendMessageUrl, this.Token, message.ChannelId ?? message.UserId,
                message.MessageText, this.UserId);

            string json = this.GetContent(url);

            var content = this.ConvertJsonIntoContent<MessageSendResponse>(json);

            bool isStatusOk = content.IsStatusOk;
            if (!isStatusOk)
            {
                throw new SlackApiHelpereException(content.Error);
            }
        }

        public virtual ICollection<Message> GetMessages() //TODO: MAKE IT GENRIC TYPE
        {
            var messages = new List<Message>();

            foreach (var channel in this.Components.Channels)
            {
                var url = string.Format(RequestUrls.ChannelsHistoryUrl, this.Token, channel.ChannelId,
                    RequestUrls.TakeMessageByRequest);

                var json = this.GetContent(url);

                var messagesInternal = this.GetMessagesInternal(json, null, channel.ChannelId);
                messages.AddRange(messagesInternal);
            }

            foreach (var im in this.Components.Ims)
            {
                var url = string.Format(RequestUrls.ImHistoryUrl, this.Token, im.ImId,
                    RequestUrls.TakeMessageByRequest);

                var json = this.GetContent(url);

                var messagesInternal = this.GetMessagesInternal(json, im.ImId, null, true);

                messages.AddRange(messagesInternal);
            }

            return messages;
        }

        internal virtual AuthResponse Auth(string token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException(nameof(token));
            }

            string url = string.Format(RequestUrls.AuthTestUrl, token);

            string json = this.GetContent(url);
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
            message.ChannelId = channelId;

            message.IsPrivateMessage = isPrivateMessage;
            message.IsGroupMessage = isGroupMessage;

            return message;
        }

        internal virtual ICollection<Bot> GetBots()
        {
            return new List<Bot>(); //TODO: READ FROM EXTERNAL JSON FILE
        }

        internal virtual ICollection<Channel> GetChannels()
        {
            var url = string.Format(RequestUrls.ChannelsListUrl, this.Token);

            string json =  this.GetContent(url);

            var content = this.ConvertJsonIntoContent<ChannelParent>(json);

            bool isStatusOk = content.IsStatusOk;
            if (!isStatusOk)
            {
                throw new SlackApiHelpereException(content.Error);
            }

            return content.Channels;
        }

        protected virtual string GetContent(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException(nameof(url));
            }

            var json = WebRequestUtility.GetContent(url);
            if (json == null)
            {
                throw new SlackApiHelpereException(nameof(json));
            }

            return json;
        }

        protected virtual ICollection<EmojiParent> GetEmojis()
        {
            //var url = string.Format(RequestConfig.EmojiUrl, this.Token);

            //Func<string> getJson = () => this.GetContent(url);

            //string json = JsonUtility.GetJson(getJson, "emoji");

            //var content = this.ConvertJsonIntoContent<EmojiParent>(json);

            //bool isStatusOk = content.IsStatusOk;
            //if (!isStatusOk)
            //{
            //    throw new SlackApiHelpereException(content.Error);
            //}

            //return null;

            return new List<EmojiParent>();
        }

        protected virtual ICollection<Im> GetIms()
        {
            var url = string.Format(RequestUrls.ImListUrl, this.Token);

            string json =  this.GetContent(url);

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

        protected virtual ICollection<User> GetUsers()
        {
            var url = string.Format(RequestUrls.UserListUrl, this.Token);

            string json = this.GetContent(url);

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