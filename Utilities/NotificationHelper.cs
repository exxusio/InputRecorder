namespace InputRecorder.Utilities
{
    /// <summary>
    /// Provides methods for displaying notifications.
    /// </summary>
    internal static class NotificationHelper
    {
        private static int timeout = 1000; // Duration for the balloon tip to be displayed in milliseconds

        private static readonly NotifyIcon _notifyIcon = new NotifyIcon
        {
            Icon = SystemIcons.Information,
            Visible = true
        };

        /// <summary>
        /// Shows a balloon notification with the specified message.
        /// </summary>
        /// <param name="message">The message to be displayed in the notification.</param>
        public static void ShowNotification(string message)
        {
            _notifyIcon.BalloonTipTitle = "Notification";
            _notifyIcon.BalloonTipText = message;
            _notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            _notifyIcon.ShowBalloonTip(timeout);
        }
    }
}