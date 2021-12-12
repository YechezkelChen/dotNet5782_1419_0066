using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalFacade
{
    public static class DalFactory
    {
        public static IDal GetDal(string typeOfdal)
        {
            switch (typeOfdal)
            {
                case "DalObject":
                    return Dal.DalObject.Instance.Value;
                case "DalXml":
                // return new BObject(); 
                default:
                    throw new Dal.FactoryException($"Please enter one from this 2 strings: 'DalObject' or 'DalXml'");
            }
        }


        //public static IDal GetDal()
        //{

        //}
    }
}
