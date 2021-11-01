using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;

namespace BL
{
    public partial class BL : IBL
    {
        public IEnumerable<Drone> drones = new List<Drone>();

        public BL()
        {
            IDal dal = new DalObject.DalObject();
            double dAvailable = 0;
            double dLightW = 0;
            double dMediumW = 0;
            double dHeavyW = 0;
            double chargingRateOfDrone = 0; //Percent per hour
            drones = dal.GetDrones();
        }
    }
}
