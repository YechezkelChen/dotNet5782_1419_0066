using System;

namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public int Id { get; set; }
            public int Name { get; set; }
            public double Longitude { get; set; }
            public double Lattitued { get; set; }
            public int ChargeSlots { get; set; }

            public override string ToString()
            {
                return $"Station #{Id}: Name = {Name}, Longitude = {Longitude},Lattitued = {Lattitued}, Number Of ChargeSlots = {ChargeSlots}";
            }
        }
    }
}

