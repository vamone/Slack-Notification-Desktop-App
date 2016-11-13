using System;
using System.IO;
using System.Reflection;

namespace Slack.Intelligence
{
    public static class ExceptionLogging
    {
        public static void Trace(Exception ex)
        {
            string debugFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string logsFolderPath = Path.Combine(debugFolder, "logs");

            bool exists = Directory.Exists(logsFolderPath);
            if (!exists)
            {
                Directory.CreateDirectory(logsFolderPath);
            }

            string path = Path.Combine(debugFolder, "logs", $"{DateTime.Today:yyyy-MM-dd}.txt");

            string error = $"{DateTime.Now} | {ex.Message} | {ex.StackTrace}{Environment.NewLine}";

            File.AppendAllText(path, error);
        }
    }
}