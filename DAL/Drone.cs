using System;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int id { get; set; }
            public string model { get; set; }
            public WeightCategories maxWeight { get; set; }
            //public DroneStatuses status { get; set; }
            //public double battry { get; set; }

            public override string ToString()
            {
                return $"Id #{id}: Model={model}, MaxWeight={maxWeight},Statuses={status}, Battry={battry}";
            }
        }
    }
}

