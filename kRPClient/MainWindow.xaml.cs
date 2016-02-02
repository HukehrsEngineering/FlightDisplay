using kRPCLib.Viewmodels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
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

            DataContext = Viewmodel = new FlightDisplayModel();
        }

        public FlightDisplayModel Viewmodel { get; set; }
        private NotificationView Notifications { get; set; }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Viewmodel.ShouldPoll = false;
        }

        private async void Notifications_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LastNotificationMessage")
            {
                StatusNotifications.Content = string.Format("{0}: {1}", DateTime.Now, Notifications.LastNotificationMessage);
            }
            else if (e.PropertyName == "LastErrorMessage")
            {
                await this.ShowMessageAsync("Error", Notifications.LastErrorMessage);
            }
        }

        private void ConnectMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Viewmodel.ConnectAndStartPolling();
            }
            catch(Exception exc)
            {
                Notifications.LastErrorMessage = string.Format("Could not connect to KRPC-Server: {0}", exc.Message);
            }
        }

        private void QuitMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}