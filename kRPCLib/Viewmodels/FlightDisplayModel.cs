using KRPC.Client;
using KRPC.Client.Services.SpaceCenter;
using System;
using System.Net;
using System.Threading;

namespace kRPCLib.Viewmodels
{
    public class FlightDisplayModel : BaseViewModel
    {
        private KRPC.Client.Connection _connection;
        private string _flightName;
        private Thread _pollingThread;
        private bool _shouldPoll;
        private Service _spaceCenter;

        public FlightDisplayModel(NotificationView notifications)
        {
            Notifications = notifications;
            MapCoordinates = new MapCoordinateView()
            {
                DisplayWidth = 640 * 2,
                DisplayHeight = 320 * 2,
                ShapeSize = 24,
                ShapeAscendingNodeSize = 12
            };
            Sensors = new SensorsViewModel();
            Resources = new ResourceView();
            Flight = new FlightViewModel();
            Orbit = new OrbitViewModel();
        }

        public FlightViewModel Flight
        {
            get;
            set;
        }

        public string FlightName
        {
            get { return _flightName; }
            set { _flightName = value; OnPropertyChanged(); }
        }

        public MapCoordinateView MapCoordinates
        {
            get;
            private set;
        }

        public NotificationView Notifications
        {
            get;
            set;
        }

        public OrbitViewModel Orbit
        {
            get;
            set;
        }

        public ResourceView Resources
        {
            get;
            private set;
        }

        public SensorsViewModel Sensors
        {
            get;
            private set;
        }

        public bool ShouldPoll
        {
            get { return _shouldPoll; }
            set { _shouldPoll = value; }
        }

        public void ConnectAndStartPolling(IPAddress address)
        {
            Connection connection = new Connection("HEL.AEROSPACE Flight Viewer", address);
            SetConnectionAndStartPolling(connection);
        }

        public void SetConnectionAndStartPolling(KRPC.Client.Connection connection)
        {
            _connection = connection;
            _spaceCenter = _connection.SpaceCenter();

            _shouldPoll = true;
            _pollingThread = new Thread(Poll);
            _pollingThread.Start();
        }

        public void UpdateFromRPC(Vessel vessel)
        {
            if (vessel == null)
            {
                return;
            }

            var parts = vessel.Parts;
            var flight = vessel.Flight();
            Orbit.Update(vessel.Orbit);
            Flight.Update(flight);
            FlightName = vessel.Name;
            Sensors.UpdateSensorData(parts.Sensors);
            MapCoordinates.Update(Flight.Latitude, Flight.Longitude, vessel.Orbit.LongitudeOfAscendingNode);
            MapCoordinates.Planet = vessel.Orbit.Body.Name;
            Resources.Update(vessel.Resources);
        }

        private void Poll()
        {
            while (_shouldPoll)
            {
                try
                {
                    UpdateFromRPC(_spaceCenter.ActiveVessel);
                }
                catch (Exception e)
                {
                    Notifications.LastNotificationMessage = string.Format("Error on update: {0}", e.Message);
                }
                Thread.Sleep(100);
            }
        }
    }
}