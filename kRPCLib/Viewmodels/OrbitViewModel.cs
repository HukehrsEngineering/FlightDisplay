using KRPC.Client.Services.SpaceCenter;
using System;

namespace kRPCLib.Viewmodels
{
    public class OrbitViewModel : BaseViewModel
    {
        private double _apoapsis;
        private double _longitudeOfAscending;
        private double _periapsis;
        private TimeSpan _timeToApoapsis;
        private TimeSpan _timeToPeriapsis;
        private double _velocity;

        public double Apoapsis
        {
            get { return _apoapsis; }
            set { _apoapsis = value; OnPropertyChanged(); }
        }

        public double LongitudeOfAscending
        {
            get { return _longitudeOfAscending; }
            set { _longitudeOfAscending = value; }
        }

        public double Periapsis
        {
            get { return _periapsis; }
            set { _periapsis = value; OnPropertyChanged(); }
        }

        public TimeSpan TimeToApoapsis
        {
            get { return _timeToApoapsis; }
            set { _timeToApoapsis = value; OnPropertyChanged(); }
        }

        public TimeSpan TimeToPeriapsis
        {
            get { return _timeToPeriapsis; }
            set { _timeToPeriapsis = value; OnPropertyChanged(); }
        }

        public double Velocity
        {
            get { return _velocity; }
            set { _velocity = value; OnPropertyChanged(); }
        }

        public void Update(Orbit orbit)
        {
            Velocity = orbit.Speed;
            Apoapsis = orbit.Apoapsis - orbit.Body.EquatorialRadius;
            Periapsis = orbit.Periapsis - orbit.Body.EquatorialRadius;
            TimeToApoapsis = TimeSpan.FromSeconds(orbit.TimeToApoapsis);
            TimeToPeriapsis = TimeSpan.FromSeconds(orbit.TimeToPeriapsis);
            LongitudeOfAscending = orbit.LongitudeOfAscendingNode;
        }
    }
}