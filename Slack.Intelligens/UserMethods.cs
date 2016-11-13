using System;
using System.Collections.Generic;
using System.Linq;

namespace Slack.Intelligence
{
    public static class UserMethods
    {
        public static string GetUserName(ICollection<User> users, string userId)
        {
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }

            var user = users.FirstOrDefault(x => x.UserId == userId);

            return user?.UserName;
        }

        public static User GetMyProfile(ICollection<User> users, string userId)
        {
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }

            var user = users.FirstOrDefault(x => x.UserId == userId);

            return user;
        }
    }
}