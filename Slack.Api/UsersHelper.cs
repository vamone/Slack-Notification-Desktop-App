using System;
using System.Collections.Generic;

using Slack.Intelligence;

namespace Slack.Api
{
    public static class UserHelper
    {
        public static ICollection<User> GetUsers(string token)
        {
            var url = new RequestUrlFactory(token);

            string json = WebRequestUtility.GetContent(url.UsersList);

            var users = GetUsersInternal(json);

            return users;
        }

        public static ICollection<User> GetUsersInternal(string json)
        {
            var content = JsonUtility.ConvertJsonIntoObject<UserParent>(json);
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            bool isStatusOk = content.IsStatusOk;
            if (!isStatusOk)
            {
                throw new SlackApiException(content.Error);
            }

            return content.Members;
        }
    }
}