using System;
using System.Linq;

namespace Slack.Intelligence
{
    public static class DateTimeUtility
    {
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();

            return dateTime;
        }

        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            var timestamp = (TimeZoneInfo.ConvertTimeToUtc(dateTime) -
                    new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

            return timestamp;
        }
    }
}