using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO ////// ???????????????????????????????????????????????????????????????????
{
    public class DalFactory
    {
        static DalApi.IDal GetDal(string dal)
        {
            DalApi.IDal dalObject = DalObject.DalObject.Instance;
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
