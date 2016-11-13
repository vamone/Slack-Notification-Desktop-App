using System;
using System.IO;
using System.Net;
using System.Text;
using Slack.Intelligens;

namespace Slack.Intelligence
{
    public class WebRequestUtility
    {
        public static string GetContent(
            string url,
            string userAgent = null,
            Encoding encoding = null)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException(nameof(url));
            }

            string webpageContent = GetHttpResponseContent(url, encoding);

            return webpageContent;
        }

        private static string GetHttpResponseContent(
            string url,
            Encoding encoding)
        {
            encoding = encoding ?? Encoding.UTF8; //TODO: SUPPORT FOR OTHER LANGUAGES

            var request = WebRequest.CreateHttp(url);

            var timeout = (int) TimeSpan.FromSeconds(10).TotalMilliseconds;

            request.Timeout = timeout;
            request.ContinueTimeout = timeout;
            request.ReadWriteTimeout = timeout;

            try
            {
                var response = request.GetResponse();

                using (var data = response.GetResponseStream())
                {
                    if (data == null)
                    {
                        return null;
                    }

                    using (var reader = new StreamReader(data, encoding))
                    {
                        string content = reader.ReadToEnd();
                        return content;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.Trace(ex);
            }

            return null;
        }
    }
}