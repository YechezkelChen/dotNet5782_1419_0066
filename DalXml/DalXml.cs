using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using DalApi;
using DalXml;

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
        private string configPath = @"config.xml";
        #endregion
        /// <summary>
        /// Get the request power consumption
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] GetRequestPowerConsumption()
        {
            XElement batteryUsages = XMLTools.LoadListFromXmlElement(configPath);

            double[] powerConsumption = new double[5];
            powerConsumption[0] = (from b in batteryUsages.Elements("BatteryUsages")
                select Convert.ToDouble(b.Element("FreeBatteryUsing").Value)).FirstOrDefault();
            powerConsumption[1] = (from b in batteryUsages.Elements("BatteryUsages")
                select Convert.ToDouble(b.Element("LightBatteryUsing").Value)).FirstOrDefault();
            powerConsumption[2] = (from b in batteryUsages.Elements("BatteryUsages")
                select Convert.ToDouble(b.Element("MediumBatteryUsing").Value)).FirstOrDefault();
            powerConsumption[3] = (from b in batteryUsages.Elements("BatteryUsages")
                select Convert.ToDouble(b.Element("HeavyBatteryUsing").Value)).FirstOrDefault();
            powerConsumption[4] = (from b in batteryUsages.Elements("BatteryUsages")
                select Convert.ToDouble(b.Element("ChargingRate").Value)).FirstOrDefault();
            return powerConsumption;
        }
    }
}
