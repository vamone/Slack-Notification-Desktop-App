using System;
using System.Linq;

using Slack.Intelligence;

namespace Slack.Notification.Service
{
    public class SlackApiHelper : SlackApiHelperBase
    {
        public InitializationResults Initialize(string token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException(nameof(token));
            }

            var init = new InitializationResults();

            try
            {
                var auth = this.Auth(token);

                bool isStatusOk = auth.IsStatusOk;
                if (!isStatusOk)
                {
                    string errorCode = auth.Error;

                    var responseError =
                        AuthResponseError.GetErrorsAndWarnings.SingleOrDefault(x => x.ErrorCode.Equals(errorCode));

                    init.Message = responseError != null
                        ? responseError.Description
                        : auth.Error;

                    init.AuthResponseError = responseError;

                    return init;
                }

                this.UserId = auth.UserId;
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
                init.Message = ex.Message;
                init.IsSuccess = false;

                return init;
            }

            init.IsSuccess = true;

            return init;
        }
    }
}