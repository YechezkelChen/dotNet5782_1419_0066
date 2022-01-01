using System;
using DalApi;

namespace Dal
{
    sealed partial class DalXml : DalApi.IDal
    {
        #region singelton
        static volatile Lazy<DalXml> instance = new Lazy<DalXml>(() => new DalXml());

        static object syncRoot = new object();
        public static DalXml Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Lazy<DalXml>(() => new DalXml());
                    }
                }

                return instance.Value;
            }
        }

        static DalXml() { }
        DalXml(){}
        #endregion

        #region DS Xml Files

        private string customersPath = @"Customers.xml";
        private string dronesPath = @"Drones.xml";
        private string dronesChargePath = @"DronesCharge.xml";
        private string parcelsPath = @"Parcels.xml";
        private string stationsPath = @"Stations.xml";


        #endregion

        public double[] GetRequestPowerConsumption()
        {
            double[] powerConsumption = new double[5];
            powerConsumption[0] = 0.05;
            powerConsumption[1] = 0.2;
            powerConsumption[2] = 0.4;
            powerConsumption[3] = 0.5;
            powerConsumption[4] = 50;
            return powerConsumption;
        }
    }
}
