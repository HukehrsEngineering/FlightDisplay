using kRPCLib.Viewmodels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Net;
using System.Windows;

namespace kRPClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Notifications = new NotificationView();
            Notifications.PropertyChanged += Notifications_PropertyChanged;

            DataContext = Viewmodel = new FlightDisplayModel(Notifications);
        }

        public FlightDisplayModel Viewmodel { get; set; }
        private NotificationView Notifications { get; set; }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Viewmodel.ShouldPoll = false;
        }

        private void Notifications_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LastNotificationMessage")
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    StatusNotifications.Content = string.Format("{0}: {1}", DateTime.Now, Notifications.LastNotificationMessage);
                }));
            }
            else if (e.PropertyName == "LastErrorMessage")
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    this.ShowMessageAsync("Error", Notifications.LastErrorMessage);
                }));
            }
        }

        private async void ConnectMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var ip = await this.ShowInputAsync("Enter IP", "Enter an IP address to connect to. Leave empty to connect to localhost.");
                IPAddress address = IPAddress.Parse(string.IsNullOrWhiteSpace(ip) ? "127.0.0.1" : ip);
                Viewmodel.ConnectAndStartPolling(address);
                SetLoadMaskVisibility(false);
            }
            catch(Exception exc)
            {
                Notifications.LastErrorMessage = string.Format("Could not connect to KRPC-Server: {0}", exc.Message);
                SetLoadMaskVisibility(true);
            }
        }

        private void QuitMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        void SetLoadMaskVisibility(bool visible)
        {
            this.Dispatcher.Invoke((Action)(() =>
                {
                    LoadMask.Visibility = visible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                }));
        }
    }
}