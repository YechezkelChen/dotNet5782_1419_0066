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

        private string customersPath = @"CustomersXml.xml";
        private string dronesPath = @"DronesXml.xml";
        private string dronesChargePath = @"DronesChargeXml.xml";
        private string paecelsPath = @"ParcelsXml.xml";
        private string stationsPath = @"StationsXml.xml";


        #endregion

        public double[] GetRequestPowerConsumption()
        {
            throw new NotImplementedException();
        }
    }
}
