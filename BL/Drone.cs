using System;
using System.Collections.Generic;
using System.Linq;

namespace IBL
{
    namespace BO
    { 
        public class Drone
        {
            public int id { get; set; }
            public string model { get; set; }
            public WeightCategories maxWeight { get; set; }


            public Location location { get; set; }
        }
    }
}
