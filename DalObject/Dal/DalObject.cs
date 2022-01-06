using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.CompilerServices;



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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] GetRequestPowerConsumption()
        {
            double[] powerConsumption = new double[5];
            powerConsumption[0] = DataSource.Config.FreeBatteryUsing;
            powerConsumption[1] = DataSource.Config.LightBatteryUsing;
            powerConsumption[2] = DataSource.Config.MediumBatteryUsing;
            powerConsumption[3] = DataSource.Config.HeavyBatteryUsing;
            powerConsumption[4] = DataSource.Config.ChargingRate;
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
