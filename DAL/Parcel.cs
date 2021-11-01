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
            public int droneId { get; set; }
            public DateTime requested { get; set; }
            public DateTime scheduled { get; set; }
            public DateTime pickedUp { get; set; }
            public DateTime delivered { get; set; }

            public override string ToString()
            {
                return $"Id #{Id}: SenderId = {SenderId}, TargetId = {TargetId},Weight = {Weight}, Priority = {Priority}, DroneId = {droneId}, Requested = {requested}, Scheduled = {scheduled}, PickedUp = {pickedUp},Delivered = {delivered}";
            }

        }
    }
}

