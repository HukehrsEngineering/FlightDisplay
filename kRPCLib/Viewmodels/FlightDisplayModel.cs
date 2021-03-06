﻿using KRPC.Client;
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
        private KRPC.Client.Services.SpaceCenter.Service _spaceCenter;

        public FlightDisplayModel(NotificationView notifications)
        {
            Notifications = notifications;
            Vessel = new VesselViewModel();
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

        public bool AreViewsEnabled
        {
            get
            {
                return !IsConnected;
            }
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

        public bool IsConnected
        {
            get
            {
                return ShouldPoll && _connection != null;
            }
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

        public VesselViewModel Vessel
        {
            get;
            set;
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
            ConnectedAndViewsVisbilityChanged();
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
            Vessel.Update(vessel);
            Sensors.UpdateSensorData(parts.Sensors);
            MapCoordinates.Update(Flight.Latitude, Flight.Longitude, vessel.Orbit.LongitudeOfAscendingNode);
            MapCoordinates.Planet = vessel.Orbit.Body.Name;
            Resources.Update(vessel.Resources, vessel.MET);
        }

        private void ConnectedAndViewsVisbilityChanged()
        {
            OnPropertyChanged("IsConnected");
            OnPropertyChanged("AreViewsEnabled");
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
                    _shouldPoll = false;
                    _connection = null;
                    _spaceCenter = null;
                    Notifications.LastErrorMessage = string.Format("Error on update: {0}", e.Message);
                    ConnectedAndViewsVisbilityChanged();
                }
                Thread.Sleep(100);
            }
        }
    }
}