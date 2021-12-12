using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public static class DalFactory
    {
        public static IDal GetDal(string dal)
        {
            IDal dalObject = Dal.DalObject.Instance.Value;
            // DalXml..............................................................

           // if (dal == "DalObject")
                return dalObject;
            //else if(dal == "DalXml")
                //return DalXml;
                //else
                //throw new FactoryException($"Please enter one from this 2 strings: 'DalObject' or 'DalXml'");

        }
    }
}
