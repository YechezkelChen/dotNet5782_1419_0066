using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace IBL
{
    namespace BO
    {
        public class Station
        {
            public int id { get; set; }
            public int name { get; set; }
            public Location location { get; set; }
            public int chargeSlots { get; set; }
            public List<DroneCharge> inCharges { get; set; }
        }
    }
}
