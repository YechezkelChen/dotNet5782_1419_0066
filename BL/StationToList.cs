using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class StationToList
        {
            public int id { get; set; }
            public int name { get; set; }
            public int chargeSlotsAvailable { get; set; }
            public int chargeSlotsNotAvailable { get; set; }
        }
    }
}