using KRPC.Client.Services.SpaceCenter;

namespace kRPCLib.Viewmodels
{
    public class FlightViewModel : BaseViewModel
    {
        private double _altitude;
        private double _latitude;
        private double _longitude;

        public double Altitude
        {
            get { return _altitude; }
            set { _altitude = value; OnPropertyChanged(); }
        }

        public double Latitude
        {
            get { return _latitude; }
            set { _latitude = value; OnPropertyChanged(); }
        }

        public double Longitude
        {
            get { return _longitude; }
            set { _longitude = value; OnPropertyChanged(); }
        }

        public void Update(Flight flight)
        {
            Altitude = flight.SurfaceAltitude;
            Latitude = flight.Latitude;
            Longitude = flight.Longitude;
        }
    }
}