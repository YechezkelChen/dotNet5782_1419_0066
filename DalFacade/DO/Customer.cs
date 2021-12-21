using System;

namespace DO
{
    public struct Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public bool Deleted { get; set; }

        public override string ToString()
        {
            return $"Id #{Id}: Name = {Name}, Phone = {Phone},Longitude = {Longitude}, Latitude = {Latitude}";
        }
    }
}

