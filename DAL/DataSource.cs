using System;
using IDAL.DO;
using DalObject;

namespace DalObject
{
    internal class DataSource
    {
        public static Drone[] Drones = new Drone[10];
        public static Station[] Stations = new Station[5];
        public static Customer[] Customers = new Customer[100];
        public static Parcel[] Parcels = new Parcel[1000];
        public static DroneCharge[] DroneCharges = new DroneCharge[100];

        internal class Config
        {
            internal static int VacantIndexD = 0;//for drone
            internal static int VacantIndexS = 0;//for station
            internal static int VacantIndexC = 0;//for Customer
            internal static int VacantIndexP = 0;//for Parcel
            internal static int VacantIndexDC = 0;//for Drone Charge
            internal static int ParcelsId = 0;
        }

        public static void Initialize()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            string[] names = new string[] { "a", "b", "c", "d", "e" };

            for (int i = 0; i < 5; i++)
                Drones[i] = new Drone { Id = rand.Next(100000000, 999999999), Model = names[rand.Next(0, 4)], MaxWeight = (WeightCategories)rand.Next(0, 2), Status = (DroneStatuses)rand.Next(0, 2), Battry = rand.Next(0, 100) };
            Config.VacantIndexD = 5;

            for (int i = 0; i < 2; i++)
                Stations[i] = new Station { Id = rand.Next(100000000, 999999999), Name = rand.Next(1, 100), Longitude = rand.Next(10, 1000), Lattitued = rand.Next(10, 1000), ChargeSlots = rand.Next(0, 100) };
            Config.VacantIndexS = 2;

            for (int i = 0; i < 10; i++)
                Customers[i] = new Customer { Id = rand.Next(100000000, 999999999), Name = names[rand.Next(0, 4)], Phone = "05" + rand.Next(10000000, 99999999), Longitude = rand.Next(10, 1000), Lattitued = rand.Next(10, 1000) };
            Config.VacantIndexC = 10;

            for (int i = 0; i < 10; i++)
                Parcels[i] = new Parcel { Id = rand.Next(100000000, 999999999), SenderId = rand.Next(100000000, 999999999), TargetId = rand.Next(100000000, 999999999), Weight = (WeightCategories)rand.Next(0, 2), Priority = (Priorities)rand.Next(0, 2), DroneId = rand.Next(100000000, 999999999), Requested = DateTime.Now, Scheduled = DateTime.Now, PickedUp = DateTime.Now, Delivered = DateTime.Now };
            Config.VacantIndexP = 10;
            Config.ParcelsId = 1000000000;//bigger frome all the ID 
        }
    }
}