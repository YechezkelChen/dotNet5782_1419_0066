using System;

namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public int id { get; set; }
            public int senderId { get; set; }
            public int targetId { get; set; }
            public WeightCategories weight { get; set; }
            public Priorities priority { get; set; }
            public int droneId { get; set; }
            public DateTime requested { get; set; }
            public DateTime scheduled { get; set; }
            public DateTime pickedUp { get; set; }
            public DateTime delivered { get; set; }

            public override string ToString()
            {
                return $"Id #{id}: SenderId = {senderId}, TargetId = {targetId},Weight = {weight}, Priority = {priority}, DroneId = {droneId}, Requested = {requested}, Scheduled = {scheduled}, PickedUp = {pickedUp},Delivered = {delivered}";
            }

        }
    }
}

