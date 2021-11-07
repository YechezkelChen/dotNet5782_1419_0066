using System;
using IDAL.DO;
using DalObject;
using System.Collections.Generic;

namespace DalObject
{
    internal class DataSource
    {
        public static List<Drone> drones = new List<Drone>();
        public static List<Station> stations = new List<Station>();
        public static List<Customer> customers = new List<Customer>();
        public static List<Parcel> parcels = new List<Parcel>();
        public static List<DroneCharge> droneCharges = new List<DroneCharge>();

        internal class Config
        {
            internal static int ParcelsId = 0;

            // km per hour
            internal static double dAvailable = 0;
            internal static double dLightW = 0;
            internal static double dMediumW = 0;
            internal static double dHeavyW = 0;
            internal static double chargingRateOfDrone = 0;
        }

        public static void Initialize()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            List<string> names = new List<string>() { "a", "b", "c", "d", "e" };

            for (int i = 0; i < 5; i++)
            {
                drones.Add(new Drone
                {
                    Id = rand.Next(100000000, 999999999),
                    Model = names[rand.Next(0, 4)],
                    Weight = (WeightCategories) rand.Next(0, 2),
                });
            }

            for (int i = 0; i < 2; i++)
            {
                stations.Add(new Station
                {
                    Id = rand.Next(100000000, 999999999),
                    Name = rand.Next(1, 100),
                    Longitude = rand.Next(10, 1000),
                    Latitude = rand.Next(10, 1000),
                    ChargeSlots = rand.Next(0, 100)
                });
            }

            for (int i = 0; i < 10; i++)
            {
                customers.Add(new Customer
                {
                    Id = rand.Next(100000000, 999999999),
                    Name = names[rand.Next(0, 4)],
                    Phone = "05" + rand.Next(10000000, 99999999),
                    Longitude = rand.Next(10, 1000),
                    Latitude = rand.Next(10, 1000)
                });
            }

            for (int i = 0; i < 10; i++)
            {
                parcels.Add(new Parcel
                {
                    Id = rand.Next(100000000, 999999999),
                    SenderId = rand.Next(100000000, 999999999),
                    TargetId = rand.Next(100000000, 999999999),
                    Weight = (WeightCategories)rand.Next(0, 2),
                    Priority = (Priorities)rand.Next(0, 2),
                    DroneId = rand.Next(100000000, 999999999),
                    Requested = DateTime.Now,
                    Scheduled = DateTime.Now,
                    PickedUp = DateTime.Now,
                    Delivered = DateTime.Now
                });
            }
            Config.ParcelsId = 1000000000;//bigger frome all the ID 
        }
    }
}