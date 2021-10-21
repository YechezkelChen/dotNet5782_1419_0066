using System;

namespace IDAL
{
    namespace DO
    {
        public struct DroneCharge
        {
            public int droneId { get; set; }
            public int stationld { get; set; }

            public override string ToString()
            {
                return $"DroneId #{droneId}: Stationld={stationld}";
            }
        }
    }
}
