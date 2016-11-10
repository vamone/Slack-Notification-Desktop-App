using System;
using System.Collections.Generic;
using System.Linq;

namespace Slack.Intelligence
{
    public static class BotMethods
    {
        public static string GetBotName(ICollection<Bot> bots, string botId)
        {
            if (bots == null)
            {
                throw new ArgumentNullException(nameof(bots));
            }

            var bot = bots.FirstOrDefault(x => x.BotId == botId);

            return bot?.BotName;
        }
    }
}