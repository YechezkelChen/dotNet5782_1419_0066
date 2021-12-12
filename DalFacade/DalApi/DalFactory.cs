using System;
using System.Reflection;


namespace DalApi
{
    public static class DalFactory
    {
        public static IDal GetDal()
        {
            string dalType = DalConfig.DalName;
            string dalPkg = DalConfig.DalPackages[dalType];
            if (dalPkg == null)
                throw new DalConfigException($"Package {dalType} is not found in packages list in dal-config.xml");

            try
            {
                Assembly.Load(dalPkg);
            }
            catch (Exception)
            {
                throw new DalConfigException("Failed to load the dal-config.xml file");
            }

            Type type = Type.GetType($"Dal.{dalPkg}, {dalPkg}");
            if (type == null)
                throw new DalConfigException($"Class {dalPkg} is not found in the {dalPkg}.dll");

            IDal dal = type.GetProperty("Instance",
                BindingFlags.Public | BindingFlags.Static).GetValue(null) as IDal;
            if(dal == null)
                throw new DalConfigException($"Class {dalPkg} is not a singleton or wrong propertry name for Instance");

            return dal;
        }
    }
}
