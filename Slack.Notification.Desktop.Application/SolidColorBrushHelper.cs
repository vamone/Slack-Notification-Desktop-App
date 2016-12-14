using System;
using System.Windows.Media;

namespace SlackDesktopBubbleApplication
{
    public static class SolidColorBrushHelper
    {
        private const string AliceBlueHexColor = "#FFF0F8FF";

        private const string SlackIconBlueHexColor = "#FF6ec9dc";

        private const string SlackIconGreenHexColor = "#FF3eb991";

        private const string SlackIconRedHexColor = "#FFe01563";

        private const string SlackIconYellowHexColor = "#FFe9a820";

        private const string SlackIconDarkBlueHexColor = "#FF0f363e";

        public static SolidColorBrush AliceBlue => GetColorFromHex(AliceBlueHexColor);

        public static SolidColorBrush SlackBlue => GetColorFromHex(SlackIconBlueHexColor);

        public static SolidColorBrush SlackGreen => GetColorFromHex(SlackIconGreenHexColor);

        public static SolidColorBrush SlackRed => GetColorFromHex(SlackIconRedHexColor);

        public static SolidColorBrush SlackYellow => GetColorFromHex(SlackIconYellowHexColor);

        public static SolidColorBrush SlackDarkBlue => GetColorFromHex(SlackIconDarkBlueHexColor);

        public static SolidColorBrush GetColorFromHex(string hexColor)
        {
            if(hexColor == null)
            {
                throw new ArgumentNullException(nameof(hexColor));
            }

            if(string.IsNullOrWhiteSpace(hexColor))
            {
                throw new ArgumentException(nameof(hexColor));
            }

            return (SolidColorBrush)(new BrushConverter().ConvertFrom(hexColor));
        }
    }
}