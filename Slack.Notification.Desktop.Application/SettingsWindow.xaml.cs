using System.Windows;

using Slack.Intelligence;

namespace SlackDesktopBubbleApplication
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void ButtonSaveToken_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TextBoxToken.Text))
            {
                MessageBox.Show("Please enter a token.");
            }

            string token = this.TextBoxToken.Text.Trim();

            RegistryUtility.Save("MyToken", token);

            var window = WindowHelper.GetWindowByClassName<MainWindow>();

            bool isInitilized = window.IsSlackInitialized();
            if (isInitilized)
            {
                window.NotificationArea.Children.Clear();

                window.Initilize();

                window.UnSetColorsOnSlackSharpIcon();

                window.ThreadGetMessageInBackground.Start();
                window.ClearNotificationAreaTimer.Start();

                this.Close();
            }
        }
    }
}