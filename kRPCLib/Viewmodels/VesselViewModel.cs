using KRPC.Client.Services.SpaceCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kRPCLib.Viewmodels
{
    public class VesselViewModel : BaseViewModel
    {
        private string _state;

        private string _type;

        public string State
        {
            get { return _state; }
            set { _state = value; OnPropertyChanged(); }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; OnPropertyChanged(); }
        }

        public void Update(Vessel vessel)
        {
            Type = vessel.Type.ToString();
            State = vessel.Situation.ToString();
        }
    }
}