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
        private IDictionary<string, ResourceTuple> _resources;
        private string[] fuelNames = new string[] { "ElectricCharge", "LiquidFuel", "Oxidizer", "SolidFuel", "MonoPropellant" };
        private string[] lifeSupportNames = new string[] { "Food", "Water", "Oxygen", "CarbonDioxide", "Waste", "WasteWater" };

        public ResourceView()
        {
            Resources = new Dictionary<string, ResourceTuple>();
            Fuels = new List<ResourceTuple>();
            LifeSupport = new List<ResourceTuple>();

            foreach (var fuelName in fuelNames)
            {
                ResourceTuple tuple = new ResourceTuple { Name = fuelName };
                Resources[fuelName] = tuple;
                Fuels.Add(tuple);
            }

            foreach (var lifeSupportName in lifeSupportNames)
            {
                ResourceTuple tuple = new ResourceTuple { Name = lifeSupportName };
                Resources[lifeSupportName] = tuple;
                LifeSupport.Add(tuple);
            }
        }

        public IList<ResourceTuple> Fuels
        {
            get;
            set;
        }

        public IList<ResourceTuple> LifeSupport
        {
            get;
            set;
        }

        public IDictionary<string, ResourceTuple> Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }

        public void Update(KRPC.Client.Services.SpaceCenter.Resources resources)
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

            OnPropertyChanged("Resources");
            OnPropertyChanged("Fuels");
        }
    }
}