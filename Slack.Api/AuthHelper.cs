using System;
using System.Linq;

using Slack.Intelligence;

namespace Slack.Api
{
    public static class AuthHelper
    {
        public static AuthResponse GetAuthResponse(string token)
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

            string json = WebRequestUtility.GetContent(url.Auth);

            var auth = GetAuthResponseInternal(json);

            return auth;
        }

        internal static AuthResponse GetAuthResponseInternal(string json)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentException(nameof(json));
            }

            var auth = JsonUtility.ConvertJsonIntoObject<AuthResponse>(json);
            if (auth == null)
            {
                throw new ArgumentNullException(nameof(auth));
            }

            if (!auth.IsStatusOk)
            {
                var responseError =
                    AuthErrorsAndWarnings.Get.SingleOrDefault(x => x.ErrorCode.Equals(auth.Error));

                string message = responseError != null
                    ? responseError.Description
                    : auth.Error;

                throw new SlackApiException(message);
            }

            return auth;
        }
    }
}