using System.Linq;
using System.Windows;

namespace Slack.Desktop.Application
{
    public static class WindowHelper
    {
        public static bool IsWindowOpened<T>(string windowName = null) where T : Window
        {
            bool isWindowOpened = string.IsNullOrWhiteSpace(windowName) ? System.Windows.Application.Current.Windows.OfType<T>().Any() : System.Windows.Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(windowName));

            return isWindowOpened;
        }

        public static T GetWindowByClassName<T>()
        {
            var window = System.Windows.Application.Current.Windows.OfType<T>().SingleOrDefault();

            return window;
        }
    }
}