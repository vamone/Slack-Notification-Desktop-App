﻿using Microsoft.Win32;

namespace Slack.Intelligence
{
    public static class RegistryUtility
    {
        private const string ApplicationKey = "Slack.Desktop.Bubble.Application.Created.By.Valentine.Monych";

        public static void Save(string name, string value)
        {
            var registry = Registry.CurrentUser.CreateSubKey(ApplicationKey);
            if (registry != null)
            {
                registry.SetValue(name, value);
                registry.Close();
            }
        }

        public static string Read(string name)
        {
            var key = Registry.CurrentUser.OpenSubKey(ApplicationKey);

            var value = key?.GetValue(name);

            return (string) value;
        }
    }
}