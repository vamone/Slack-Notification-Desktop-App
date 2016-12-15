using System;
using System.Collections.Generic;

using Slack.Intelligence;

namespace Slack.Api
{
    public static class ImHelper
    {
        public static ICollection<Im> GetIms(string token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException(nameof(token));
            }

            var url = new RequestUrlFactory(token);

            string json = WebRequestUtility.GetContent(url.ImsList);

            var ims = GetImsFromJson(json);

            return ims;
        }

        public static ICollection<Im> GetImsFromJson(string json)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentException(nameof(json));
            }

            var ims = JsonUtility.ConvertJsonIntoObject<ImParent>(json);
            if (ims == null)
            {
                throw new ArgumentNullException(nameof(ims));
            }

            if (!ims.IsStatusOk)
            {
                throw new SlackApiException(ims.Error);
            }

            return ims.Ims;
        }
    }
}