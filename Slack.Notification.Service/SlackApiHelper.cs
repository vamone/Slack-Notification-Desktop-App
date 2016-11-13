using System;
using System.Linq;
using System.Threading.Tasks;

namespace Slack.Notification.Service
{
    public class SlackApiHelper : SlackApiHelperBase
    {
        public InitializationResult InitializeComponents(string token)
        {
            var components = new InitializationResult();

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

                    components.Result.Message = responseError != null
                        ? responseError.Description
                        : content.Error;

                    components.ResponseError = responseError;

                    return components;
                }

                this.UserId = content.UserId;

                this.Token = token;

                var channels = this.GetChannels();
                var ims = this.GetIms();
                var users = this.GetUsers();
                var bots = this.GetBots();
                var emojis = this.GetEmojis();

                this.Channels = channels;
                this.Ims = ims;
                this.Users = users;
                this.Bots = bots;
                this.Emojis = emojis;

                components.Components.Channels = channels;
                components.Components.Ims = ims;
                components.Components.Users = users;
                components.Components.Bots = bots;
                components.Components.Emojis = emojis;
            }
            catch (Exception ex)
            {
                components.Result.Message = ex.Message;
            }

            components.Result.IsSuccess = true;

            return components;
        }
    }
}