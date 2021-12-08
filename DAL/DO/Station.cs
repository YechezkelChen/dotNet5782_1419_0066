using System;

namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public int ChargeSlots { get; set; }
            
            public override string ToString()
            {
                return $"Station #{Id}: Name = {Name}, Longitude = {Longitude},Latitude = {Latitude}, Number Of ChargeSlots = {ChargeSlots}";
            }
        }
    }
}

