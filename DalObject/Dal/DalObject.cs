using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Dal
{
    sealed partial class DalObject : DalApi.IDal
    {
        #region singelton
        static volatile Lazy<DalObject> instance = new Lazy<DalObject>(() => new DalObject());

        static object syncRoot = new object();
        public static DalObject Instance
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

                return instance.Value;
            }
        }

        static DalObject() { }
        DalObject()
        {
            DataSource.Initialize();

            // For the data in files

            //string customersPath = @"Customers.xml";
            //string dronesPath = @"Drones.xml";
            //string parcelsPath = @"Parcels.xml";
            //string stationsPath = @"Stations.xml";
            //SaveListToXmlSerializer(DataSource.Customers, customersPath);
            //SaveListToXmlSerializer(DataSource.Drones, dronesPath);
            //SaveListToXmlSerializer(DataSource.Parcels, parcelsPath);
            //SaveListToXmlSerializer(DataSource.Stations, stationsPath);
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

        public static void SaveListToXmlSerializer<T>(IEnumerable<T> list, string filePath)
        {
            string dirPath = @"Data\";
            try
            {
                FileStream file = new FileStream(dirPath + filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }
    }
}
