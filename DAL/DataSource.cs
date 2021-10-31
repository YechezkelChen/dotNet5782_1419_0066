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
            internal static double dAvailable = 0;
            internal static double dLightW = 0;
            internal static double dMediumW = 0;
            internal static double dHeavyW = 0;
            internal static double chargingRateOfDrone = 0;//Percent per hour
        }

        public static void Initialize()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            List<string> names = new List<string>() { "a", "b", "c", "d", "e" };

            for (int i = 0; i < 5; i++)
            {
                drones.Add(new Drone
                {
                    id = rand.Next(100000000, 999999999),
                    model = names[rand.Next(0, 4)],
                    maxWeight = (WeightCategories) rand.Next(0, 2),
                });
            }

            for (int i = 0; i < 2; i++)
            {
                stations.Add(new Station
                {
                    id = rand.Next(100000000, 999999999),
                    name = rand.Next(1, 100),
                    longitude = rand.Next(10, 1000),
                    lattitued = rand.Next(10, 1000),
                    chargeSlots = rand.Next(0, 100)
                });
            }

            for (int i = 0; i < 10; i++)
            {
                customers.Add(new Customer
                {
                    id = rand.Next(100000000, 999999999),
                    name = names[rand.Next(0, 4)],
                    phone = "05" + rand.Next(10000000, 99999999),
                    longitude = rand.Next(10, 1000),
                    lattitued = rand.Next(10, 1000)
                });
            }

            for (int i = 0; i < 10; i++)
            {
                parcels.Add(new Parcel
                {
                    id = rand.Next(100000000, 999999999),
                    senderId = rand.Next(100000000, 999999999),
                    targetId = rand.Next(100000000, 999999999),
                    weight = (WeightCategories)rand.Next(0, 2),
                    priority = (Priorities)rand.Next(0, 2),
                    droneId = rand.Next(100000000, 999999999),
                    requested = DateTime.Now,
                    scheduled = DateTime.Now,
                    pickedUp = DateTime.Now,
                    delivered = DateTime.Now
                });
            }
            Config.ParcelsId = 1000000000;//bigger frome all the ID 
        }
    }
}