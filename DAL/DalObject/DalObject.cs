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
    sealed partial class DalObject : DalApi.IDal
    {
        #region singelton
        internal static volatile Lazy<DalObject> instance = new Lazy<DalObject>(() => new DalObject());

        private static object syncRoot = new object();
        public static Lazy<DalObject> Instance 
        { 
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Lazy<DalObject>(() => new DalObject());
                    }
                }

                return instance;
            }
        }
        static DalObject() { }
        DalObject()
        {
            DataSource.Initialize();
        }

        #endregion

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
