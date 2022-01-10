using System;


namespace BO
{
    public class Location
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public override string ToString()
        {
            return "(" + ConvertLatitude(Latitude) + ", " + ConvertLongitude(Longitude) + ")";
        }
        public static string ConvertLatitude(double coord)   /// Funcs to Convert lattitudes and longtitudes from decimal to Degrees
        {
            char direction;
            double sec = (double)Math.Round(coord * 3600);
            double deg = Math.Abs(sec / 3600);
            sec = Math.Abs(sec % 3600);
            double min = sec / 60;
            sec %= 60;
            if (coord >= 0)
                direction = 'N';
            else
                direction = 'S';
            return $"{(int)deg}° {(int)min}' {sec}'' { direction}";
        }
        public static string ConvertLongitude(double coord)
        {
            char direction;
            double sec = (double)Math.Round(coord * 3600);
            double deg = Math.Abs(sec / 3600);
            sec = Math.Abs(sec % 3600);
            double min = sec / 60;
            sec %= 60;
            if (coord >= 0)
                direction = 'E';
            else
                direction = 'W';
            return $"{(int)deg}° {(int)min}' {sec}'' { direction}";
        }
    }
}
