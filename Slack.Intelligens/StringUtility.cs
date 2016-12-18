namespace Slack.Intelligence
{
    public static class StringUtility
    {
        public static string UppercaseFirst(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            string trimmedText = text.Trim().ToLowerInvariant();

            return char.ToUpper(trimmedText[0]) + trimmedText.Substring(1);
        }
    }
}