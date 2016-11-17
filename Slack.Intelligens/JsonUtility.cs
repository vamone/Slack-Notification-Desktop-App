using System;
using System.IO;
using System.Reflection;

namespace Slack.Intelligence
{
    public static class JsonUtility
    {
        public static string GetJson(Func<string> getJsonContent, string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(nameof(fileName));
            }

            string jsonFromFile = ReadJsonFromFile(fileName);
            if (string.IsNullOrWhiteSpace(jsonFromFile))
            {
                string json = getJsonContent.Invoke();

                SaveJsonToFile(json, fileName);

                return json;
            }

            return jsonFromFile;
        }

        internal static void SaveJsonToFile(string json, string fileName)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;

            string debugFolder = Path.GetDirectoryName(assemblyLocation);
            if (debugFolder == null)
            {
                throw new DirectoryNotFoundException(nameof(debugFolder));
            }

            string folderPath = Path.Combine(debugFolder, "json");

            bool exists = Directory.Exists(folderPath);
            if (!exists)
            {
                Directory.CreateDirectory(folderPath);
            }

            string fullPath = Path.Combine(folderPath, $"{fileName}.json");

            File.WriteAllText(fullPath, json);
        }

        internal static string ReadJsonFromFile(string fileName)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;

            string debugFolder = Path.GetDirectoryName(assemblyLocation);
            if (debugFolder == null)
            {
                throw new DirectoryNotFoundException(nameof(debugFolder));
            }

            string folderPath = Path.Combine(debugFolder, "json");

            string fullPath = Path.Combine(folderPath, $"{fileName}.json");

            if (!File.Exists(fullPath))
            {
                return null;
            }

            return File.ReadAllText(fullPath);
        }
    }
}