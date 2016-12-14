using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slack.Api
{
    public class ImHelper : IHelper
    {
        public static ICollection<Im> GetIms(string token)
        {
            throw new NotImplementedException();
        }

        public static ICollection<Im> GetIms(string token)
        {
            var url = string.Format(RequestUrls.ImListUrl, this.Token);

            string json = this.GetContent(url);

            var content = this.ConvertJsonIntoContent<ImParent>(json);

            bool isStatusOk = content.IsStatusOk;
            if (!isStatusOk)
            {
                throw new SlackApiHelpereException(content.Error);
            }

            return content.Ims;
        }
    }
}
