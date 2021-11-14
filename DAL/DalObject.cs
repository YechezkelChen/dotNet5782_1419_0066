using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;
using DalObject;

namespace DalObject
{
    public partial class DalObject : IDal
    {
        public DalObject()
        {
            DataSource.Initialize();
        }

        public double[] GetRequestPowerConsumption()
        {
            double[] powerConsumption = new double[5];
            powerConsumption[0] = DataSource.Config.dAvailable;
            powerConsumption[1] = DataSource.Config.dLightW;
            powerConsumption[2] = DataSource.Config.dMediumW;
            powerConsumption[3] = DataSource.Config.dHeavyW;
            powerConsumption[4] = DataSource.Config.chargingRateOfDrone;
            return powerConsumption;
        }
    }
}
