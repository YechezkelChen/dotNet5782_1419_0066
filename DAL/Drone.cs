using System;

namespace IDAL
{
	namespace DO
	{
        public struct Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public DroneStatuses Status { get; set; }
            public double Battry { get; set; }

            public override string ToString()
            {
                return $"Id #{Id}: Model={Model}, MaxWeight={MaxWeight},Statuses={Statuses}, Battry={Battry}";
            }
        }
    }
}

