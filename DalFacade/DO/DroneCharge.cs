using System;

namespace DO
{
    public struct DroneCharge
    {
        public int DroneId { get; set; }
        public int StationId { get; set; }
        public DateTime? StartCharging { get; set; }
        public bool Deleted { get; set; }
        public override string ToString()
        {
            return $"DroneId #{DroneId}: StationId={StationId}";
        }
    }
}
