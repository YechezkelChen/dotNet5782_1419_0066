using System;

namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public int DroneId { get; set; }
            public DateTime Requested { get; set; }
            public DateTime Scheduled { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }

            public override string ToString()
            {
                return $"Id #{Id}: SenderId = {SenderId}, TargetId = {TargetId},Weight = {Weight}, Priority = {Priority}, DroneId = {DroneId}, Requested = {Requested}, Scheduled = {Scheduled}, PickedUp = {PickedUp},Delivered = {Delivered}";
            }

        }
    }
}

