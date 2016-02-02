namespace kRPCLib.Viewmodels
{
    public class NotificationView : BaseViewModel
    {
        private string _lastErrorMessage;
        private string _lastNotificationMessage;

        public string LastErrorMessage
        {
            get { return _lastErrorMessage; }
            set { _lastErrorMessage = value; OnPropertyChanged(); }
        }

        public string LastNotificationMessage
        {
            get { return _lastNotificationMessage; }
            set { _lastNotificationMessage = value; OnPropertyChanged(); }
        }
    }
}