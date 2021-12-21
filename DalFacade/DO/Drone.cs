using System;

namespace DO
{
    public struct Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories Weight { get; set; }
        public bool Deleted { get; set; }
        public override string ToString()
        {
            return $"Id #{Id}: Model={Model}, Weight={Weight}";
        }
    }
}