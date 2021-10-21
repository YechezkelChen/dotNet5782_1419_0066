using System;

namespace IDAL
{
    namespace DO
    {
        public struct Customer
        {
            public int id { get; set; }
            public string name { get; set; }
            public string phone { get; set; }
            public double longitude { get; set; }
            public double lattitued { get; set; }

            public override string ToString()
            {
                return $"Id #{id}: Name = {name}, Phone = {phone},Longitude = {longitude}, Lattitued = {lattitued}";
            }
        }
    }
}

