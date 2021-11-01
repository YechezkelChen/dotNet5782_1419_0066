using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class ParcelToList
        {
            public int id { get; set; }
            public int senderId { get; set; }
            public int targetId { get; set; }
            public WeightCategories weight { get; set; }
            public Priorities priority { get; set; }
            public ParcelStatuses parcelStatuses { get; set; }
        }
    }
}
