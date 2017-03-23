using OxyPlot.Axes;
using System.Collections.Generic;

namespace kRPCLib.Viewmodels
{
    public class ResourceTuple : BaseViewModel
    {
        private float _ammount;
        private float _maximum;
        private string _name;

        public float Ammount
        {
            get { return _ammount; }
            set { _ammount = value; OnPropertyChanged(); OnPropertyChanged("Ratio"); }
        }

        public float Maximum
        {
            get { return _maximum; }
            set { _maximum = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        public float Ratio
        {
            get { return _ammount / _maximum; }
        }
    }

    public class ResourceView : BaseViewModel
    {
        private PlotViewModel _fuelsPlot;

        private PlotViewModel _lifeSupportPlot;

        private IDictionary<string, ResourceTuple> _resources;

        private string[] fuelNames = new string[] { "ElectricCharge", "LiquidFuel", "Oxidizer", "SolidFuel", "MonoPropellant", "Kerosene" };

        private string[] lifeSupportNames = new string[] { "Food", "Waste", "Water", "WasteWater", "Oxygen", "CarbonDioxide" };

        public ResourceView()
        {
            Resources = new Dictionary<string, ResourceTuple>();
            Fuels = new List<ResourceTuple>();
            LifeSupport = new List<ResourceTuple>();

            var lifeSupportPlot = new PlotViewModel(LineMode.LastN, 100, new LinearAxis());
            var fuelsPlot = new PlotViewModel(LineMode.Continuous, 300, new LinearAxis());

            foreach (var fuelName in fuelNames)
            {
                ResourceTuple tuple = new ResourceTuple { Name = fuelName };
                Resources[fuelName] = tuple;
                Fuels.Add(tuple);
                fuelsPlot.AddSeries(fuelName);
            }

            foreach (var lifeSupportName in lifeSupportNames)
            {
                ResourceTuple tuple = new ResourceTuple { Name = lifeSupportName };
                Resources[lifeSupportName] = tuple;
                LifeSupport.Add(tuple);
                lifeSupportPlot.AddSeries(lifeSupportName);
            }

            LifeSupportPlot = lifeSupportPlot;
            FuelsPlot = fuelsPlot;
        }

        public IList<ResourceTuple> Fuels
        {
            get;
            set;
        }

        public PlotViewModel FuelsPlot
        {
            get { return _fuelsPlot; }
            set { _fuelsPlot = value; OnPropertyChanged(); }
        }

        public IList<ResourceTuple> LifeSupport
        {
            get;
            set;
        }

        public PlotViewModel LifeSupportPlot
        {
            get { return _lifeSupportPlot; }
            set { _lifeSupportPlot = value; OnPropertyChanged(); }
        }

        public IDictionary<string, ResourceTuple> Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }

        public void Update(KRPC.Client.Services.SpaceCenter.Resources resources, double MET)
        {
            foreach (var name in resources.Names)
            {
                ResourceTuple tuple;
                if (Resources.ContainsKey(name))
                {
                    tuple = Resources[name];
                }
                else
                {
                    tuple = new ResourceTuple() { Name = name };

                    Resources[name] = tuple;
                }

                tuple.Ammount = resources.Amount(name);
                tuple.Maximum = resources.Max(name);
            }

            UpdateFuelPlots(resources, MET);
            UpdateLifeSupportPlots(resources, MET);

            OnPropertyChanged("Resources");
            OnPropertyChanged("Fuels");
        }

        private void UpdateFuelPlots(KRPC.Client.Services.SpaceCenter.Resources resources, double MET)
        {
            foreach (var fuelName in fuelNames)
            {
                FuelsPlot.AddToSeries(fuelName, MET, resources.Amount(fuelName) / resources.Max(fuelName));
            }
            FuelsPlot.InvalidatePlot();
        }

        private void UpdateLifeSupportPlots(KRPC.Client.Services.SpaceCenter.Resources resources, double MET)
        {
            foreach (var lifeSupportName in lifeSupportNames)
            {
                LifeSupportPlot.AddToSeriesIfLastXHigherThan(lifeSupportName, MET, resources.Amount(lifeSupportName) / resources.Max(lifeSupportName), 300);
            }
            LifeSupportPlot.InvalidatePlot();
        }
    }
}