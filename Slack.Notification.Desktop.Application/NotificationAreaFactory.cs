using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SlackDesktopBubbleApplication
{
    public static class NotificationAreaFactory
    {
        public static Border BuildNotification(string message, double timestamp, string userName = null,
            string channelName = null)
        {
            var grid = new Grid
            {
                Cursor = Cursors.Hand,
                Width = 230,
                Margin = new Thickness(5)
            };

            var stackPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                Orientation = Orientation.Vertical,
            };

            var stackPanelHorisontal = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Width = 220
            };

            var textBlockChannelName = new TextBlock
            {
                Text = string.IsNullOrWhiteSpace(channelName) ? string.Empty : $"#{channelName}".ToLowerInvariant(),
                Foreground = new SolidColorBrush(Colors.WhiteSmoke),
                TextAlignment = TextAlignment.Left,
                Width = 110
            };

            var textBlockUserName = new TextBlock
            {
                Text = string.IsNullOrWhiteSpace(userName) ? string.Empty : $"@{userName}".ToLowerInvariant(),
                Foreground = new SolidColorBrush(Colors.WhiteSmoke),
                TextAlignment = TextAlignment.Right,
                Width = 110
            };

            stackPanelHorisontal.Children.Add(textBlockChannelName);
            stackPanelHorisontal.Children.Add(textBlockUserName);

            var textBlockMessage = new TextBlock
            {
                Text = StringUtility.UppercaseFirst(message),
                FontStyle = FontStyles.Normal,
                Margin = new Thickness {Left = 5, Bottom = 5},
                Foreground = new SolidColorBrush(Colors.WhiteSmoke),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                FontSize = 15
            };

            stackPanel.Children.Add(textBlockMessage);
            stackPanel.Children.Add(stackPanelHorisontal);

            grid.Children.Add(stackPanel);

            var border = new Border
            {
                Background = new SolidColorBrush(Colors.Black),
                Opacity = 0.70,
                CornerRadius = new CornerRadius(10, 10, 10, 10),
                Margin = new Thickness {Left = 5, Top = 5, Right = 5, Bottom = 5},
                Uid = timestamp.ToString(CultureInfo.InvariantCulture),
                Child = grid
            };

            return border;
        }
    }
}