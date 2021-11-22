using System;
using IDAL.DO;
using DalObject;
using System.Collections.Generic;

namespace DalObject
{
    internal class DataSource
    {
        public static List<Drone> Drones = new List<Drone>();
        public static List<Station> Stations = new List<Station>();
        public static List<Customer> Customers = new List<Customer>();
        public static List<Parcel> Parcels = new List<Parcel>();
        public static List<DroneCharge> DroneCharges = new List<DroneCharge>();

        internal class Config
        {
            internal static int ParcelsId = 1;

            // km per hour
            internal static double BatteryAvailable = 0.05;
            internal static double BatteryLightWeight = 1;
            internal static double BatteryMediumWeight = 1.5;
            internal static double BatteryHeavyWeight = 2;
            internal static double ChargingRateOfDrone = 1;
        }

        public static void Initialize()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            List<string> names = new List<string>() { "a", "b", "c", "d", "e" };
            List<DateTime?> dates = new List<DateTime?>() {DateTime.Now, null};

            for (int i = 0; i < 5; i++)
            {
                Drones.Add(new Drone
                {
                    Id = rand.Next(100000000, 1000000000),
                    Model = names[rand.Next(0, 5)],
                    Weight = (WeightCategories) rand.Next(0, 3),
                });
            }

            for (int i = 0; i < 2; i++)
            {
                Stations.Add(new Station
                {
                    Id = rand.Next(100000000, 1000000000),
                    Name = rand.Next(1, 100),
                    Longitude = rand.NextDouble(),
                    Latitude = rand.NextDouble(),
                    ChargeSlots = rand.Next(0, 10)
                });
            }

            for (int i = 0; i < 10; i++)
            {
                int tmpId = rand.Next(10000000, 100000000);
                tmpId = tmpId * 10 + lastDigitID(tmpId);
                Customers.Add(new Customer
                {
                    Id = tmpId,
                    Name = names[rand.Next(0, 5)],
                    Phone = "05" + rand.Next(10000000, 99999999),
                    Longitude = rand.NextDouble(),
                    Latitude = rand.NextDouble()
                });
            }

            for (int i = 0; i < 10; i++)
            {
                Parcels.Add(new Parcel
                {
                    Id = Config.ParcelsId,
                    SenderId = Customers[rand.Next(0, 10)].Id,
                    TargetId = Customers[rand.Next(0, 10)].Id,
                    Weight = (WeightCategories)rand.Next(0, 3),
                    Priority = (Priorities)rand.Next(0, 3), 
                    DroneId = Drones[rand.Next(0,5)].Id,
                    Requested = dates[rand.Next(0, 2)], 
                    Scheduled = dates[rand.Next(0, 2)],
                    PickedUp = dates[rand.Next(0, 2)],
                    Delivered = dates[rand.Next(0, 2)]
                });
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