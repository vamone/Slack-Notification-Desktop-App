using System;
using System.Collections.Generic;
using System.Linq;

namespace Slack.Api
{
    public static class ProfileHelper
    {
        public static User GetProfile(ICollection<User> users, string userId)
        {
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }

            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException(nameof(userId));
            }

            var user = users.FirstOrDefault(x => x.UserId == userId);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user;
        }
    }
}