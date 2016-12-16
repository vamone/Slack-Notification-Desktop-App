using System;
using System.Collections.Generic;

using Slack.Intelligence;

namespace Slack.Api
{
    public static class MessageHelper
    {
        public static ICollection<Message> GetMessages(string token, string channelId, bool isChannelsMessages = false)
        {
            var factory = new RequestUrlFactory(token, channelId);

            string url = isChannelsMessages ? factory.ChannelHistory : factory.ImHistory;

            string json = WebRequestUtility.GetContent(url);

            var messages = GetMessagesInternal(json);

            return messages;
        }

        public static ICollection<Message> GetMessagesInternal(string json)
        {
            var content = JsonUtility.ConvertJsonIntoObject<MessageParent>(json);
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            bool isStatusOk = content.IsStatusOk;
            if (!isStatusOk)
            {
                throw new SlackApiException(content.Error);
            }

            return content.Messages;
        }

        public static bool IsMessageVisible(Message message, DateTime lastMessageAt, string profileId)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (string.IsNullOrWhiteSpace(message.MessageText))
            {
                return false;
            }

            var messageInDateTime = DateTimeUtility.UnixTimeStampToDateTime(message.Timestamp);

            bool isMessageNew = lastMessageAt < messageInDateTime;
            bool isOwnMessage = message.UserId == profileId;

            bool isMessageVisible = isMessageNew && !isOwnMessage;
            if (isMessageVisible)
            {
                return true;
            }

            return false;
        }

        internal static Message FormatMessage(Message message, Components components, string token, string imId = null,
            string channelId = null,
            bool isPrivateMessage = false,
            bool isChannelMessage = false)
        {
            string userName = UserMethods.GetUserName(components.Users, message.UserId);
            if (userName == null)
            {
                components.Users = UserHelper.GetUsers(token);

                userName = UserMethods.GetUserName(components.Users, message.UserId);
            }

            string botName = BotMethods.GetBotName(components.Bots, message.BotId);
            if (botName == null)
            {
                components.Bots = BotsHelper.GetBots(token);

                botName = BotMethods.GetBotName(components.Bots, message.BotId);
            }

            string imName = ImMethos.GetImName(components.Ims, components.Users, imId);
            if (imName == null)
            {
                components.Ims = ImHelper.GetIms(token);

                imName = ImMethos.GetImName(components.Ims, components.Users, imId);
            }

            string channelName = ChannelMethods.GetChannelName(components.Channels, channelId);
            if (channelName == null)
            {
                components.Channels = ChannelHelper.GetChannels(token);

                channelName = ChannelMethods.GetChannelName(components.Channels, channelId);
            }

            message.UserName = botName ?? userName;
            message.ChannelName = isPrivateMessage ? null : (imName ?? channelName);
            message.ChannelId = channelId;

            message.IsPrivateMessage = isPrivateMessage;
            message.IsGroupMessage = isChannelMessage;

            return message;
        }

        public static bool IsMessageSended(Message message, string token)
        {
            string url =
                new RequestUrlFactory(token, message.ChannelId, message.MessageText, message.UserId).SendMessage;

            string json = WebRequestUtility.GetContent(url);

            return IsMessagesSendedFromJson(json);
        }

        public static bool IsMessagesSendedFromJson(string json)
        {
            var content = JsonUtility.ConvertJsonIntoObject<MessageSendResponse>(json);
            if (content == null)
            {
                return false;
            }

            bool isStatusOk = content.IsStatusOk;
            if (!isStatusOk)
            {
                return false;
            }

            return true;
        }
    }
}