using System;
using System.Linq;

namespace Slack.Api
{
    public class AuthHelper : IHelper
    {
        public static AuthResponse GetAuthResponse(string token, IWebRequest webRequest = null)
        {
            var url = RequestUrlFactory.BuildUrl(token, RequestUrlFactory.AuthTestUrl);

            string json = webRequest == null ? WebRequestUtility.GetContent(url) : webRequest.GetContent(url);

            var auth = GetAuthResponseInternal(json);

            return auth;
        }

        public static void Validate(AuthResponse auth)
        {
            if (auth == null)
            {
                throw new ArgumentNullException(nameof(auth));
            }

            bool isStatusOk = auth.IsStatusOk;
            if (!isStatusOk)
            {
                var responseError =
                    AuthErrorsAndWarnings.Get.SingleOrDefault(x => x.ErrorCode.Equals(auth.Error));

                string message = responseError != null
                    ? responseError.Description
                    : auth.Error;

                throw new AuthException(message);
            }
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

            return auth;
        }
    }
}