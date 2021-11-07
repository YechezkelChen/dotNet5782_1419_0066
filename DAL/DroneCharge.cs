using System;

namespace IDAL
{
    namespace DO
    {
        public struct DroneCharge
        {
            public int DdroneId { get; set; }
            public int Stationld { get; set; }
            public int ArrivingTimeToCharge { get; set; }

            public override string ToString()
            {
                return $"DroneId #{DdroneId}: Stationld={Stationld}";
            }
        }
    }
}
