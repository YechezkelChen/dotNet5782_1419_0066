using System;


namespace BO
{
    public class Location
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public override string ToString()
        {
            return $"Longitude = " + String.Format("{0:0.000}", Longitude) + "\n" +
                   $"Latitude = " + String.Format("{0:0.000}", Latitude) + "\n";
        }
    }
}
