using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalObject;

namespace DalObject
{
    public partial class DalObject : DalApi.IDal
    {
        public DalObject()
        {
            DataSource.Initialize();
        }

        public double[] GetRequestPowerConsumption()
        {
            double[] powerConsumption = new double[5];
            powerConsumption[0] = DataSource.Config.BatteryAvailable;
            powerConsumption[1] = DataSource.Config.BatteryLightWeight;
            powerConsumption[2] = DataSource.Config.BatteryMediumWeight;
            powerConsumption[3] = DataSource.Config.BatteryHeavyWeight;
            powerConsumption[4] = DataSource.Config.ChargingRateOfDrone;
            return powerConsumption;
        }
    }
}
