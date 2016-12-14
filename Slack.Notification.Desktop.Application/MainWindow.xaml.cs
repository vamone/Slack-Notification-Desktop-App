using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using Slack;
using Slack.Intelligence;
using Slack.Notification.Service;

namespace SlackDesktopBubbleApplication
{
    public partial class MainWindow : Window
    {
        internal DispatcherTimer ClearNotificationAreaTimer;

        static readonly Lazy<SlackApiHelper> SlackLazy = new Lazy<SlackApiHelper>();

        static SlackApiHelper Slack => SlackLazy.Value;

        private Func<Message, Action> AddOrRemoveMessages;

        //private bool HasAnyExceptions = false;

        public MainWindow()
        {
            this.InitializeComponent();

            AddOrRemoveMessages =
                (m) => (Action) Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action<StackPanel>(x => this.AddOrRemoveChildren(x, m)),
                    this.NotificationArea);

            this.StackPanelControlls.Visibility = Visibility.Hidden;
            //this.GridOpenMessageMenu.Visibility = Visibility.Hidden;

            this.MainIconArea.MouseDown += OnMouseDown;
            this.MainIconArea.MouseLeave += OnMouseLeave;

            this.ClearNotificationAreaTimer = new DispatcherTimer();
            this.ClearNotificationAreaTimer.Tick += ClearNotificationsTimerEventProcessor;
            this.ClearNotificationAreaTimer.Interval = new TimeSpan(0, 0, 5);

            this.TextBoxChannelGroupImName.TextChanged += RenderChannelImOrGroupNameChangedText;
        }

        private void RenderChannelImOrGroupNameChangedText(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            var reg = new Regex("([#,@])(.*)");
            var text = reg.Match(textBox.Text).Groups[2].Value;

            var bb = text;

            //var channelsNames = Slack.Components.Channels.Select(x => x.ChannelName).ToList();

            //var channels =
            //    channelsNames.Where(
            //        x => x.StartsWith(text, StringComparison.InvariantCultureIgnoreCase));
        }

        internal void AddOrRemoveChildren(StackPanel stackPanelNotificationArea, Message message)
        {
            this.SetColorsOnSlackSharpIcon();

            var elements = stackPanelNotificationArea.Children.Cast<Border>().ToList();

            double totaltHeightElements = elements.Select(x => x.ActualHeight).Sum();

            if (totaltHeightElements > 200)
            {
                var lastElement = elements.FirstOrDefault();

                stackPanelNotificationArea.Children.Remove(lastElement);
            }

            var notification = NotificationAreaFactory.BuildNotification(message.MessageText,
                message.Timestamp,
                message.UserName,
                message.ChannelName);

            stackPanelNotificationArea.Children.Add(notification);
        }

        internal void GetAndDisplayMessages(Func<Message, Action> action)
        {
            while (true)
            {
                try
                {
                    var messages = Slack.GetMessages();

                    foreach (var message in messages)
                    {
                        action.Invoke(message);
                    }
                }
                catch (SlackApiHelpereException ex)
                {
                    if (!this.HasAnyExceptions)
                    {
                        MessageHelper.AddMessage(action,
                            $"Error: {ex.Message}", "system");

                        ExceptionLogging.Trace(ex);
                    }

                    this.HasAnyExceptions = true;

                    this.ClearNotificationAreaTimer.Stop();
                }
                catch (Exception ex)
                {
                    if (!this.HasAnyExceptions)
                    {
                        MessageHelper.AddMessage(action,
                            $"Error: {ex.Message}", "system");

                        ExceptionLogging.Trace(ex);
                    }

                    this.HasAnyExceptions = true;

                    this.ClearNotificationAreaTimer.Stop();
                }
            }
        }

        internal bool IsSlackInitialized()
        {
            string token = RegistryUtility.Read("MyToken");

            var init = Slack.Initialize(token);

            bool hasInitSuccess = init.Status.IsSuccess;
            if (!hasInitSuccess)
            {
                string messageText =
                    $"Error: {init.Status.Message}\nSolution: {init.AuthResponseError.SolutionMessage}";

                MessageHelper.AddMessage(this.AddOrRemoveMessages, messageText, "system");

                //TODO: START RED LINE BLINK

                return false;
            }

            return true;
        }

        internal void SetColorsOnSlackSharpIcon()
        {
            this.SlackLineGreen.Background = SolidColorBrushUtility.SlackGreen;
            this.SlackLineYellow.Background = SolidColorBrushUtility.SlackYellow;
            this.SlackLineBlue.Background = SolidColorBrushUtility.SlackBlue;
            this.SlackLineRed.Background = SolidColorBrushUtility.SlackRed;
        }

        internal void SetInactiveColorsOnSlackShartIcon()
        {
            this.SlackLineGreen.Background = SolidColorBrushUtility.SlackDarkBlue;
            this.SlackLineYellow.Background = SolidColorBrushUtility.SlackDarkBlue;
            this.SlackLineBlue.Background = SolidColorBrushUtility.SlackDarkBlue;
            this.SlackLineRed.Background = SolidColorBrushUtility.SlackDarkBlue;
        }

        internal void UnSetColorsOnSlackSharpIcon()
        {
            this.SlackLineGreen.Background = SolidColorBrushUtility.AliceBlue;
            this.SlackLineYellow.Background = SolidColorBrushUtility.AliceBlue;
            this.SlackLineBlue.Background = SolidColorBrushUtility.AliceBlue;
            this.SlackLineRed.Background = SolidColorBrushUtility.AliceBlue;
        }

        //internal void SetWarningColorsOnSlackSharpIcon()
        //{
        //    var thread = new Thread(() =>
        //    {
        //        Dispatcher.Invoke(DispatcherPriority.Normal,
        //            new Action<Border>(this.SetCCC),
        //            this.SlackLineBlue);
        //    });

        //    thread.Start();
        //}

        //internal void SetCCC(Border slackSharpIcon)
        //{
        //    while (true)
        //    {
        //        slackSharpIcon.Background = SolidColorBrushUtility.SlackRed;
        //        Thread.Sleep(300);

        //        slackSharpIcon.Background = SolidColorBrushUtility.SlackDarkBlue;
        //        Thread.Sleep(300);
        //    }
        //}

        private void ClearNotificationsTimerEventProcessor(object sender, EventArgs e)
        {
            var elements =
                this.NotificationArea.Children.Cast<object>()
                    .Select(x => x as FrameworkElement)
                    .Where(x => x != null)
                    .ToList();

            foreach (var element in elements)
            {
                double messageTimestamp = Convert.ToDouble(element.Uid);

                var messageAddedAt = DateTimeUtility.UnixTimeStampToDateTime(messageTimestamp);

                bool isSomething = messageAddedAt.AddSeconds(10) <= DateTime.Now;
                if (isSomething)
                {
                    for (int j = 0; j < 300; j++)
                    {
                        double value = 300 - j;
                        element.Margin = new Thickness {Left = value};
                    }

                    this.NotificationArea.Children.Remove(element);
                }
            }

            if (!elements.Any())
            {
                this.UnSetColorsOnSlackSharpIcon();
            }
        }

        private void GridMainIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            this.StackPanelControlls.Visibility = Visibility.Visible;
        }

        private void GridMainIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            //TODO: FIND BETTER LOGIC AS MANY MOUSE OVER HIDES MENU WHEN IT'S NOT A TIME
            var thread = new Thread(() =>
            {
                Thread.Sleep(3000);

                Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action<StackPanel>(this.SetVisibilityHidden),
                    this.StackPanelControlls);
            });

            thread.Start();
        }

        private void GridOpenMessageMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Feature -> Write message here.");
        }

        private void GridOpenSettingsMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bool isSettingsWindowOpened = WindowHelper.IsWindowOpened<SettingsWindow>();
            if (isSettingsWindowOpened)
            {
                var window = WindowHelper.GetWindowByClassName<SettingsWindow>();

                window.Close();

                return;
            }

            var settingsWindow = new SettingsWindow();

            settingsWindow.Show();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }

            if (e.RightButton == MouseButtonState.Pressed)
            {
                MessageHelper.AddMessage(this.AddOrRemoveMessages, $"You are logged in as: @{Slack.MyPofile.UserName}",
                    "system");
            }

            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.ClickCount >= 2)
                {
                    this.HasAnyExceptions = false;

                    MessageHelper.AddMessage(this.AddOrRemoveMessages, "Bubble has been restarted.", "system");

                    this.ClearNotificationAreaTimer.Start();
                }
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
        }

        private void SetVisibilityHidden(StackPanel stackPanel)
        {
            stackPanel.Visibility = Visibility.Hidden;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            try
            {
                bool isSlackInitilized = this.IsSlackInitialized();
                if (!isSlackInitilized)
                {
                    return;
                }

                var thread = new Thread(() => this.GetAndDisplayMessages(this.AddOrRemoveMessages));
                thread.Start();

                this.ClearNotificationAreaTimer?.Start();
            }
            catch (Exception ex)
            {
                ExceptionLogging.Trace(ex);

                MessageHelper.AddMessage(this.AddOrRemoveMessages, $"Error: {ex.Message}", "system");
            }
            finally
            {
                this.SetInactiveColorsOnSlackShartIcon();
            }
        }

        private void TextBoxChannelGroupImName_KeyUp(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;

            return;
            
            string name = this.TextBoxChannelGroupImName.Text;
            if (string.IsNullOrWhiteSpace(name) && name.Length < 3)
            {
                return;
            }

            //string tempName = name;

            //if (!string.IsNullOrWhiteSpace(name))
            //{
            //    tempName = name.Replace("#", string.Empty).Replace("@", string.Empty);
            //}

            var channelsNames = Slack.Components.Channels.Select(x => x.ChannelName).ToList();

            var channels =
                channelsNames.Where(
                    x => x.StartsWith(name, StringComparison.InvariantCultureIgnoreCase));

            if (channels.Count() > 1)
            {
                return;
            }

            if (channels.Count() == 1)
            {
                textBox.Text = $"#{channels.SingleOrDefault()}";
            }
        }

        private void TextBoxChannelGroupImName_KeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;

            var reg = new Regex("([#,@])(.*)");
            var text = reg.Match(textBox.Text).Groups[2].Value;

            //var channelsNames = Slack.Components.Channels.Select(x => x.ChannelName).ToList();

            //var channels =
            //    channelsNames.Where(
            //        x => x.StartsWith(text, StringComparison.InvariantCultureIgnoreCase));


            return;
        }
    }
}