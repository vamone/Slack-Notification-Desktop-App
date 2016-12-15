using System.Collections.Generic;

namespace Slack.Api
{
    public static class BotsHelper
    {
        public static ICollection<Bot> GetBots(string token)
        {
            return new List<Bot>();
        }

        public static ICollection<Bot> GetBotsFromJson(string json = null)
        {
            return new List<Bot>();
        } 
    }
}