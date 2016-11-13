using System;
using System.IO;
using System.Net;
using System.Text;

namespace Slack.Intelligence
{
    public static class WebRequestUtility
    {
        public static string GetContent(
            string url,
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

            string content = GetHttpResponseContent(url, encoding);

            return content;
        }

        internal static string GetHttpResponseContent(
            string url,
            Encoding encoding)
        {
            encoding = encoding ?? Encoding.UTF8; //TODO: SUPPORT FOR OTHER LANGUAGES

            var request = WebRequest.CreateHttp(url);

            var timeout = (int) TimeSpan.FromSeconds(10).TotalMilliseconds;

            request.Timeout = timeout;
            request.ContinueTimeout = timeout;
            request.ReadWriteTimeout = timeout;

            var response = request.GetResponse();

            using (var data = response.GetResponseStream())
            {
                if (data == null)
                {
                    return null;
                }

                using (var reader = new StreamReader(data, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}