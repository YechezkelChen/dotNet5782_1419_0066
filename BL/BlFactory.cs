﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO ///????????????????????????????????????????????
{
    class BlFactory
    {
        public BlApi.IBL GetBl()
        {
            BlApi.IBL bl =  DalObject.DalObject.Instance;
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
