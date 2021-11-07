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
            public int Id { get; set; }
            public int Name { get; set; }
            public int ChargeSlotsAvailable { get; set; }
            public int ChargeSlotsNotAvailable { get; set; }
            public override string ToString()
            {
                return $"Id #{Id}: Name = {Name},Charge slots available = {ChargeSlotsAvailable}," +
                       $"charge slots not available = {ChargeSlotsNotAvailable}";
            }
        }
    }
}