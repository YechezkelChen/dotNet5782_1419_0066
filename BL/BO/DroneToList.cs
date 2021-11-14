using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class DroneToList
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories Weight { get; set; }
            public double Battery { get; set; }
            public DroneStatuses Status { get; set; }
            public Location Location { get; set; }
            public int IdParcel { get; set; }
            public override string ToString()
            {
                return
                    $"Id #{Id}: Model = {Model}, Weight = {Weight}, Battery = {Battery}, " +
                    $"Drone Statues = {Status}, Location = {Location},Id of the parcel = {IdParcel}";
            }
        }
    }
}