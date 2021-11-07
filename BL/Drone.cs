using System;
using System.Collections.Generic;
using System.Linq;

namespace IBL
{
    namespace BO
    { 
        public class Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories Weight { get; set; }
            public double Battery { get; set; }
            public DroneStatuses Status { get; set; }
            public DeliveryByTransfer DeliveryByTransfer { get; set; }
            public Location Location { get; set; }

            public override string ToString()
            {
                return
                    $"Id #{Id}: Model = {Model}, Weight = {Weight}," +
                    $"Battery = {Battery}," +
                    $"Statuse = {Status}," +
                    $"DeliveryByTransfer = {DeliveryByTransfer}," +
                    $"Location = {Location}";
            }
        }
    }
}
