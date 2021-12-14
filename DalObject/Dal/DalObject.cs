using System;


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
