using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kRPCLib.Viewmodels
{
    public enum LineMode
    {
        Continuous,
        LastN,
        MaxN
    }

    public class PlotViewModel : BaseViewModel
    {
        private int _dataPoints;

        private Dictionary<string, DataPoint> _lastPointOfSeries;

        private LineMode _mode;

        private PlotModel _plot;

        private Dictionary<string, LineSeries> _series;

        private string _title;

        public PlotViewModel(LineMode mode, int dataPointsCount, Axis yAxis)
        {
            Mode = mode;
            DataPointsCount = dataPointsCount;
            _series = new Dictionary<string, LineSeries>();
            _lastPointOfSeries = new Dictionary<string, DataPoint>();

            LinearAxis xAxis = new LinearAxis();
            xAxis.Position = AxisPosition.Bottom;
            xAxis.Title = "Time";

            yAxis.Position = AxisPosition.Left;
            yAxis.Title = "Values";

            var plot = new PlotModel
            {
                Title = Title,
                TitleHorizontalAlignment = TitleHorizontalAlignment.CenteredWithinPlotArea,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.TopCenter
            };

            plot.Axes.Add(xAxis);
            plot.Axes.Add(yAxis);

            Plot = plot;
        }

        public int DataPointsCount
        {
            get { return _dataPoints; }
            set { _dataPoints = value; OnPropertyChanged(); }
        }

        public Dictionary<string, DataPoint> LastPointOfSeries
        {
            get { return _lastPointOfSeries; }
            set { _lastPointOfSeries = value; }
        }

        public LineMode Mode
        {
            get { return _mode; }
            set { _mode = value; OnPropertyChanged(); }
        }

        public PlotModel Plot
        {
            get { return _plot; }
            set { _plot = value; OnPropertyChanged(); }
        }

        public Dictionary<string, LineSeries> Series
        {
            get { return _series; }
            set { _series = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }

        public void AddSeries(string seriesName)
        {
            LineSeries s = new LineSeries()
            {
                Title = seriesName
            };
            Series[seriesName] = s;
            LastPointOfSeries[seriesName] = new DataPoint(double.MinValue, 0);
            Plot.Series.Add(s);
        }

        public void AddToSeries(string seriesName, double x, double y)
        {
            AddToSeries(seriesName, new DataPoint(x, y));
        }

        public void AddToSeriesIfLastXHigherThan(string seriesName, double x, double y, double lastXHigher)
        {
            if (LastPointOfSeries[seriesName].X < x - lastXHigher)
            {
                AddToSeries(seriesName, new DataPoint(x, y));
            }
        }

        public void InvalidatePlot()
        {
            Plot.InvalidatePlot(true);
        }

        private void AddToSeries(string seriesName, DataPoint p)
        {
            LineSeries series = Series[seriesName];
            if (series.Points.Count > DataPointsCount)
            {
                switch (Mode)
                {
                    case LineMode.Continuous:
                        AverageNPoints(series.Points, DataPointsCount, DataPointsCount / 4);
                        break;

                    case LineMode.LastN:
                        var ring = series.Points.Skip(Math.Max(0, series.Points.Count - DataPointsCount)).ToList();
                        series.Points.Clear();
                        series.Points.AddRange(ring);
                        break;

                    case LineMode.MaxN:
                        break;
                }
            }
            series.Points.Add(p);
            LastPointOfSeries[seriesName] = p;
        }

        private void AverageNPoints(List<DataPoint> list, int numPoints, int averageFirst)
        {
            IList<DataPoint> points = new List<DataPoint>();

            List<DataPoint> tempList = new List<DataPoint>();

            points.Add(list[0]);

            averageFirst++;

            for (int i = 1; i < list.Count; i++)
            {
                if (i == averageFirst || (i < averageFirst && tempList.Count == numPoints))
                {
                    points.Add(new DataPoint(list[i].X,
                        tempList.Average(d => d.Y)
                        ));
                    tempList.Clear();
                }
                tempList.Add(list[i]);
            }

            list.Clear();
            list.AddRange(points);
        }
    }
}