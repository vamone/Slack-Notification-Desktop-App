using System;

using Newtonsoft.Json;

namespace Slack.Api
{
    public class JsonUtility
    {
        internal static T ConvertJsonIntoObject<T>(string json)
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
                throw new ArgumentNullException(nameof(content));
            }

            return content;
        }
    }
}