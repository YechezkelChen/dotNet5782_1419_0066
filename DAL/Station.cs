using System;

namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public int id { get; set; }
            public int name { get; set; }
            public double longitude { get; set; }
            public double lattitued { get; set; }
            public int chargeSlots { get; set; }

            public override string ToString()
            {
                return $"Station #{id}: Name = {name}, Longitude = {longitude},Lattitued = {lattitued}, Number Of ChargeSlots = {chargeSlots}";
            }
        }
    }
}

