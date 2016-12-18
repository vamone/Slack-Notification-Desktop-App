using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace Slack.Desktop.Application.Updater
{
    public class UpdateHelper
    {
        public const string UpdateCheckUrl = "http://monych.se/bubbles/slack/update/";

        public static bool HasUpdates()
        {
            try
            {
                var updateVersion = GetUpdateInformation();
                if (updateVersion == null)
                {
                    return false;
                }

                int latestVerstion = Convert.ToInt32(updateVersion.Version.Replace(".", string.Empty));

                var version = Assembly.GetExecutingAssembly().GetName().Version;

                int currentVersion = Convert.ToInt32($"{version.Major}{version.Minor}{version.Build}{version.Revision}");

                if (currentVersion < latestVerstion)
                {
                    return true;
                }
            }
            catch //TODO: LOGG EXCEPTIONS
            {
                return false;
            }

            return false;
        }

        public static bool HasDownloadedFile(string url, string fileName)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(new Uri(url), fileName);

                    File.SetAttributes(fileName, FileAttributes.Normal);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static UpdateInformation GetUpdateInformation()
        {
            string json = WebRequestUtility.GetContent($"{UpdateCheckUrl}");
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            var updateVersion = JsonUtility.ConvertJsonIntoObject<UpdateInformation>(json);
            if (updateVersion == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            return updateVersion;
        }
    }
}