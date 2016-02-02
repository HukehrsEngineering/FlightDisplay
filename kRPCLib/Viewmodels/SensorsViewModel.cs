using KRPC.Client.Services.SpaceCenter;
using System.Collections.Generic;

namespace kRPCLib.Viewmodels
{
    public class SensorsViewModel : BaseViewModel
    {
        private static string NO_DATA_STRING = "NO DATA";
        private string _acceleration;
        private string _g;
        private string _pressure;
        private string _temperature;

        public SensorsViewModel()
        {
            Pressure = NO_DATA_STRING;
            G = NO_DATA_STRING;
            Temperature = NO_DATA_STRING;
            Acceleration = NO_DATA_STRING;
        }

        public string Acceleration
        {
            get { return _acceleration; }
            set { _acceleration = value; OnPropertyChanged(); }
        }

        public string G
        {
            get { return _g; }
            set { _g = value; OnPropertyChanged(); }
        }

        public string Pressure
        {
            get { return _pressure; }
            set { _pressure = value; OnPropertyChanged(); }
        }

        public string Temperature
        {
            get { return _temperature; }
            set { _temperature = value; OnPropertyChanged(); }
        }

        public void UpdateSensorData(IList<Sensor> sensors)
        {
            if (sensors == null)
            {
                return;
            }

            foreach (var sensor in sensors)
            {
                switch (sensor.Part.Name)
                {
                    case "sensorBarometer":
                        Pressure = sensor.Value;
                        break;

                    case "sensorThermometer":
                        Temperature = sensor.Value;
                        break;

                    case "sensorGravimeter":
                        Acceleration = sensor.Value;
                        break;

                    case "sensorAccelerometer":
                        G = sensor.Value;
                        break;
                }
            }
        }
    }
}