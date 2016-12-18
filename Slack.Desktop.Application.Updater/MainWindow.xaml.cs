using System;
using System.Windows;

namespace Slack.Desktop.Application.Updater
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            //this.LabelProgressBarTitle.Visibility = Visibility.Hidden;
            //this.ProressBarDownloadStatus.Visibility = Visibility.Hidden;
            //this.LabelDownloadStatus.Visibility = Visibility.Hidden;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var latest = UpdateHelper.GetUpdateInformation();

            this.LabelUpdateVersion.Content = $"Version: {latest.Version}";
        }

        private void ButtonDownloadAndUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ButtonDownloadAndUpdate.Visibility = Visibility.Hidden;

                this.LabelProgressBarTitle.Visibility = Visibility.Visible;
                this.ProressBarDownloadStatus.Visibility = Visibility.Visible;

                var latest = UpdateHelper.GetUpdateInformation();

                int ratio = 100/latest.Files.Count;

                foreach (var file in latest.Files)
                {
                    var url = $"{latest.DownloadUrl}{latest.Version}/{file.Name}";

                    bool hasFiledDownloaded = UpdateHelper.HasDownloadedFile(url, file.Name);
                    if (hasFiledDownloaded)
                    {
                        this.ProressBarDownloadStatus.Value = this.ProressBarDownloadStatus.Value + ratio;

                        this.LabelDownloadStatus.Visibility = Visibility.Visible;
                        this.LabelDownloadStatus.Content = this.ProressBarDownloadStatus.Value + ratio + "%";
                    }
                }

                double progressBarValue = this.ProressBarDownloadStatus.Value;
                if (progressBarValue != 100)
                {
                    this.ButtonDownloadAndUpdate.Visibility = Visibility.Visible;

                    this.LabelDownloadStatus.Visibility = Visibility.Visible;
                    this.LabelDownloadStatus.Content = "Update failed. Please try again";
                }
                else
                {
                    this.LabelDownloadStatus.Visibility = Visibility.Visible;
                    this.LabelDownloadStatus.Content = "Application successfully updated";
                }
            }
            catch (Exception ex)
            {
                this.LabelDownloadStatus.Visibility = Visibility.Visible;
                this.LabelDownloadStatus.Content = "Update failed. Please try again";
            }
        }
    }
}