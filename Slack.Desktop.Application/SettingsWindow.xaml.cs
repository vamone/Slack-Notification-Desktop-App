using System.Windows;

using Slack.Intelligence;

namespace Slack.Desktop.Application
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

            var window = WindowHelper.GetWindowByClassName<Slack.Desktop.Application.MainWindow>();

            bool isInitilized = window.IsSlackInitialized();
            if (isInitilized)
            {
                window.NotificationArea.Children.Clear();

                window.UnSetColorsOnSlackSharpIcon();

                this.Close();
            }
        }
    }
}