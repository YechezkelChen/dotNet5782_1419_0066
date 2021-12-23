using System;


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
        public DateTime? Requested { get; set; }
        public DateTime? Scheduled { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }
        public override string ToString()
        {
            return $"Id #{Id}: Sender = {Sender}, Target = {Target}, Weight = {Weight}," +
                   $" Priority = {Priority}, Drone in Parcel = {DroneInParcel}, Requested = {Requested}," +
                   $" Scheduled = {Scheduled}, PickedUp = {PickedUp},Delivered = {Delivered},";
        }
    }
}
