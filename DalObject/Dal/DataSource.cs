using System;
using System.Collections.Generic;
using DO;


namespace Dal
{
    internal class DataSource
    {
        internal static List<Drone> Drones = new List<Drone>();
        internal static List<Station> Stations = new List<Station>();
        internal static List<Customer> Customers = new List<Customer>();
        internal static List<Parcel> Parcels = new List<Parcel>();
        internal static List<DroneCharge> DroneCharges = new List<DroneCharge>();

        internal class Config
        {
            internal static int ParcelsId = 100000;

            // km per hour
            internal static double BatteryAvailable = 0.05;
            internal static double BatteryLightWeight = 0.2;
            internal static double BatteryMediumWeight = 0.4;
            internal static double BatteryHeavyWeight = 0.5;
            internal static double ChargingRateOfDrone = 50;
        }

        public static void Initialize()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            List<string> droneNames = new List<string>() { "Topacc", "Mavic", "Phantom", "Combo", "Tello" };
            List<string> stationNames = new List<string>() { "Bar Ilan", "Artist", "Jaffa", "Carmel", "Palmach" };
            List<string> customerNames = new List<string>() { "Abraham", "Isaac", "Jacob", "Moshe", "Joseph" };
            List<DateTime?> dates = new List<DateTime?>() {DateTime.Now, null};

            int id = 100000;
            for (int i = 0; i < 10; i++)
            {
                Drones.Add(new Drone
                {
                    Id = id,
                    Model = droneNames[rand.Next(0, 5)],
                    Weight = (WeightCategories) rand.Next(0, 3),
                });
                id += 10;
            }

            for (int i = 0; i < 10; i++)
            {
                Stations.Add(new Station
                {
                    Id = id,
                    Name = stationNames[rand.Next(0, 5)],
                    Longitude = rand.NextDouble(),
                    Latitude = rand.NextDouble(),
                    AvailableChargeSlots = rand.Next(0, 10)
                });
                id += 10;
            }

            id = 100000000;
            for (int i = 0; i < 10; i++)
            {
                int tmpId = id;
                tmpId = tmpId * 10 + lastDigitID(tmpId);
                Customers.Add(new Customer
                {
                    Id = tmpId,
                    Name = customerNames[rand.Next(0, 5)],
                    Phone = "05" + rand.Next(10000000, 99999999),
                    Longitude = rand.NextDouble(),
                    Latitude = rand.NextDouble()
                });
                id += 10;
            }

            for (int i = 0; i < 10; i++)
            {
                Parcel parcel = new Parcel()
                {
                    Id = Config.ParcelsId,
                    SenderId = Customers[rand.Next(0, 10)].Id,
                    TargetId = Customers[rand.Next(0, 10)].Id,
                    Weight = (WeightCategories) rand.Next(0, 3),
                    Priority = (Priorities) rand.Next(0, 3),
                    DroneId = Drones[rand.Next(0, 5)].Id,
                    Requested = dates[rand.Next(0, 2)],
                    Scheduled = dates[rand.Next(0, 2)],
                    PickedUp = dates[rand.Next(0, 2)],
                    Delivered = dates[rand.Next(0, 2)]
                };

                if (parcel.Delivered != null)
                {
                    parcel.Requested = DateTime.Now;
                    parcel.Scheduled = DateTime.Now;
                    parcel.PickedUp = DateTime.Now;
                }
                else if (parcel.PickedUp != null)
                {
                    parcel.Requested = DateTime.Now;
                    parcel.Scheduled = DateTime.Now;
                }
                else if (parcel.Scheduled != null)
                    parcel.Requested = DateTime.Now;

                Parcels.Add(parcel);
                Config.ParcelsId++;
            }
        }

        private static int lastDigitID(int lessID)
        {
            int digit1, digit2, sumResultDigits = 0, digitID;
            for (int i = 1; i <= lessID; i++)
            {
                digit1 = lessID % 10;
                digit1 *= 2;//Calculating the digits double their weight.
                sumResultDigits += sumDigits(digit1);//The sum of the result digits.
                lessID /= 10;
                digit2 = lessID % 10;
                digit2 *= 1;//Calculating the digits double their weight.
                sumResultDigits += sumDigits(digit2);//The sum of the result digits.
                lessID /= 10;
            }
            sumResultDigits %= 10;//The unity digit of the result.

            digitID = 10 - sumResultDigits;
            return digitID;//Returning the missing digit.v
        }

        private static int sumDigits(int num)//Entering a number by the computer.
        {
            int sum_digits = 0;
            while (num > 0)
            {
                sum_digits += num % 10;
                num = num / 10;
            }
            return sum_digits;//Return of the sum of his digits.
        }
    }
}