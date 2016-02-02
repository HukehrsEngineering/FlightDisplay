using System;
using System.Linq;

namespace kRPCLib.Viewmodels
{
    public class MapCoordinateView : BaseViewModel
    {
        private static readonly string[] AvailableMaps = new string[] { "kerbin", "mun" };
        private static readonly string DefaultMap = "Empty";

        private double _displayHeight;
        private double _displayWidth;
        private string _mapImagePath;
        private string _planet;
        private double _positionAscendingNodeX;
        private double _positionAscendingNodeY;
        private double _positionX;
        private double _positionY;
        private double _shapeAscendingNodeSize;

        public MapCoordinateView()
        {
            Planet = "Kerbin";
        }

        public double DisplayHeight
        {
            get { return _displayHeight; }
            set { _displayHeight = value; }
        }

        public double DisplayWidth
        {
            get { return _displayWidth; }
            set { _displayWidth = value; }
        }

        public string MapImagePath
        {
            get { return _mapImagePath; }
            set
            {
                _mapImagePath = value;
                OnPropertyChanged();
            }
        }

        public string Planet
        {
            get { return _planet; }
            set
            {
                bool updateMapPath = _planet != value;
                _planet = value;
                OnPropertyChanged();
                if (updateMapPath)
                {
                    MapImagePath = string.Format(
                        "/Maps/{0}.jpg",
                        AvailableMaps.Contains(value.ToLower()) ? value : DefaultMap);
                }
            }
        }

        public double PositionAscendingNodeX
        {
            get { return _positionAscendingNodeX; }
            set { _positionAscendingNodeX = value; OnPropertyChanged(); }
        }

        public double PositionAscendingNodeY
        {
            get { return _positionAscendingNodeY; }
            set { _positionAscendingNodeY = value; OnPropertyChanged(); }
        }

        public double PositionX
        {
            get { return _positionX; }
            set { _positionX = value; OnPropertyChanged(); }
        }

        public double PositionY
        {
            get { return _positionY; }
            set { _positionY = value; OnPropertyChanged(); }
        }

        public double ShapeAscendingNodeSize
        {
            get { return _shapeAscendingNodeSize; }
            set
            {
                _shapeAscendingNodeSize = value;
                PositionAscendingNodeY = (_displayHeight / 2) - (value / 2);
            }
        }

        public double ShapeSize { get; set; }

        public void Update(double latitude, double longitude, double longitudeOfAscending)
        {
            //latitude: north (90) to south (-90)
            //lontitude: east (180) to west (-180)
            PositionY = (((latitude + 90) / 180) * DisplayHeight) - (ShapeSize / 2);
            PositionX = (((longitude + 180) / 360) * DisplayWidth) - (ShapeSize / 2);
            var ascendingInDeg = RadianToDegree(longitudeOfAscending);
            PositionAscendingNodeX = ((ascendingInDeg / 360) * DisplayWidth) - (ShapeAscendingNodeSize / 2);
        }

        private double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }
}