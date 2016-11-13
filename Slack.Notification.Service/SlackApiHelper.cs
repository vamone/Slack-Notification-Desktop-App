using System;
using System.Linq;

using Slack.Intelligence;

namespace Slack.Notification.Service
{
    public class SlackApiHelper : SlackApiHelperBase
    {
        public InitializationResult Initialize(string token)
        {
            var init = new InitializationResult();

            try
            {
                string url = string.Format(RequestConfig.AuthTestUrl, token);

                string json = this.GetContent(url);

                if (json == null)
                {
                    throw new ArgumentNullException(nameof(json));
                }

                if (string.IsNullOrWhiteSpace(json))
                {
                    throw new ArgumentException(nameof(json));
                }

                var content = this.ConvertJsonIntoContent<AuthResponse>(json);

                if (content == null)
                {
                    throw new ArgumentNullException(nameof(content));
                }

                bool isStatusOk = content.IsStatusOk;
                if (!isStatusOk)
                {
                    string errorCode = content.Error;

                    var responseError =
                        AuthResponseError.GetErrorsAndWarnings.SingleOrDefault(x => x.ErrorCode.Equals(errorCode));

                    init.Result.Message = responseError != null
                        ? responseError.Description
                        : content.Error;

                    init.ResponseError = responseError;

                    return init;
                }

                this.UserId = content.UserId;
                this.Token = token;

                var users = this.GetUsers();

                this.MyPofile = UserMethods.GetMyProfile(users, this.UserId);

                this.Components.Channels = this.GetChannels();
                this.Components.Ims = this.GetIms();
                this.Components.Users = users;
                this.Components.Bots = this.GetBots();
                this.Components.Emojis = this.GetEmojis();
            }
            catch (Exception ex)
            {
                init.Result.Message = ex.Message;
                init.Result.IsSuccess = false;

                return init;
            }

            init.Result.IsSuccess = true;

            return init;
        }
    }
}