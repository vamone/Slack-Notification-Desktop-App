using System;
using System.Collections.Generic;
using System.Linq;

namespace Slack.Intelligence
{
    public static class ImMethos
    {
        public static string GetImName(ICollection<Im> ims, ICollection<User> users, string imId)
        {
            if (ims == null)
            {
                throw new ArgumentNullException(nameof(ims));
            }

            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }

            var im = ims.FirstOrDefault(x => x.ImId == imId);
            if (im == null)
            {
                return null;
            }

            var user = users.FirstOrDefault(x => x.UserId == im.UserId);

            return user?.UserName;
        }
    }
}