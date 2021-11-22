using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Parcel
        {
            public int Id { get; set; }
            public CustomerInParcel Sender { get; set; }
            public CustomerInParcel Target { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public DroneInParcel DroneInParcel { get; set; }
            public DateTime? Requested { get; set; }//יצירה
            public DateTime? Scheduled { get; set; }//שיוך
            public DateTime? PickedUp { get; set; }//איסוף
            public DateTime? Delivered { get; set; }//אספקה
            public override string ToString()
            {
                return $"Id #{Id}: SenderId = {Sender}, TargetId = {Target}, Weight = {Weight}," +
                       $" Priority = {Priority}, Drone in Parcel = {DroneInParcel}, Requested = {Requested}," +
                       $" Scheduled = {Scheduled}, PickedUp = {PickedUp},Delivered = {Delivered},";
            }
        }
    }
}
