using System;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int Id { get; set; }
            public string model { get; set; }
            public WeightCategories maxWeight { get; set; }

            public override string ToString()
            {
                return $"Id #{Id}: Model={model}, MaxWeight={maxWeight}";
            }
        }
    }
}