using System;
using System.Collections.Generic;
using System.Linq;


namespace BO
{
    public class Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories Weight { get; set; }
        public double Battery { get; set; }
        public DroneStatuses Status { get; set; }
        public ParcelByTransfer ParcelByTransfer { get; set; }
        public Location Location { get; set; }

        public override string ToString()
        {
            return
                $"Id #{Id}:\n" +
                $"Model = {Model}\n" +
                $"Weight = {Weight}\n" +
                $"Battery = {Battery}\n" +
                $"Status = {Status}\n" +
                $"DeliveryByTransfer = {ParcelByTransfer}" +
                $"Location = {Location}";
        }
    }
}
