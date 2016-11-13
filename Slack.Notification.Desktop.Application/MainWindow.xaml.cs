using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using Slack;
using Slack.Intelligence;
using Slack.Intelligens;
using Slack.Notification.Service;

namespace SlackDesktopBubbleApplication
{
    public partial class MainWindow : Window
    {
        internal DispatcherTimer ClearNotificationAreaTimer;

        internal Thread ThreadGetMessageInBackground;

        private static readonly SlackApiHelper Slack = new SlackApiHelper();

        private Func<Message, Action> AddMessageAction;

        private bool IsTestMode = false;

        public MainWindow()
        {
            this.InitializeComponent();
            this.InitilizeBubbleApplication();
        }

        internal void InitilizeBubbleApplication()
        {
            try
            {
                AddMessageAction =
                    (m) => (Action)Dispatcher.Invoke(DispatcherPriority.Normal,
                        new Action<StackPanel>(x => this.AddChildren(x, m)),
                        this.NotificationArea);

                this.StackPanelControlls.Visibility = Visibility.Hidden;
                this.GridOpenMessageMenu.Visibility = Visibility.Hidden;

                this.MainIconArea.MouseDown += OnMouseDown;
                this.MainIconArea.MouseLeave += OnMouseLeave;

                bool isSlackInitilized = this.IsSlackInitialized();
                if (!isSlackInitilized)
                {
                    return;
                }

                this.ThreadGetMessageInBackground = new Thread(() => this.GetAndDisplayMessages(this.AddMessageAction));

                this.ClearNotificationAreaTimer = new DispatcherTimer();
                this.ClearNotificationAreaTimer.Tick += ClearNotificationsTimerEventProcessor;
                this.ClearNotificationAreaTimer.Interval = new TimeSpan(0, 0, 5);
            }
            catch (Exception ex)
            {
                ExceptionLogging.Trace(ex);

                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                this.SetInactiveColorsOnSlackShartIcon();
            }
        }

        internal void AddChildren(StackPanel stackPanelNotificationArea, Message message)
        {
            this.SetColorsOnSlackSharpIcon();

            var elements = stackPanelNotificationArea.Children.Cast<Border>();

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
            var messages = this.IsTestMode ? MockMessages.GetMockMessages() : GetMessagesContinuouslyInternal();

            foreach (var message in messages)
            {
                action.Invoke(message);
            }
        }

        internal IEnumerable<Message> GetMessagesContinuouslyInternal()
        {
            var messages = Slack.GetMessages();

            foreach (var message in messages)
            {
                yield return message;
            }

            var nextMessages = GetMessagesContinuouslyInternal();

            foreach (var message in nextMessages)
            {
                yield return message;
            }
        }

        internal bool IsSlackInitialized()
        {
            string token = RegistryUtility.Read("MyToken");

            var components = Slack.InitializeComponents(token);

            bool hasInitSuccess = components.Result.IsSuccess;
            if (!hasInitSuccess)
            {
                string messageText =
                    $"Error: {components.Result.Message}\nSolution: {components.ResponseError.SolutionMessage}";

                MessageHelper.AddMessage(this.AddMessageAction, messageText, "system");

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
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
        }

        private void SetVisibilityHidden(StackPanel stackPanel)
        {
            stackPanel.Visibility = Visibility.Hidden;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.ThreadGetMessageInBackground != null)
            {
                this.ThreadGetMessageInBackground.Start();
            }

            if (this.ClearNotificationAreaTimer != null)
            {
                this.ClearNotificationAreaTimer.Start();
            }
        }
    }
}