using System;
using IDAL.DO;
using DalObject;

namespace DalObject
{
    internal class DataSource
    {
        public static Drone[] drones = new Drone[10];
        public static Station[] stations = new Station[5];
        public static Customer[] customers = new Customer[100];
        public static Parcel[] parcels = new Parcel[1000];
        public static DroneCharge[] droneCharges = new DroneCharge[100];
        
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
                drones[i] = new Drone { id = rand.Next(100000000, 999999999), model = names[rand.Next(0, 4)], maxWeight = (WeightCategories)rand.Next(0, 2), status = (DroneStatuses)rand.Next(0, 2), battry = rand.Next(0, 100) };
            Config.VacantIndexD = 5;

            for (int i = 0; i < 2; i++)
                stations[i] = new Station { id = rand.Next(100000000, 999999999), name = rand.Next(1, 100), longitude = rand.Next(10, 1000), lattitued = rand.Next(10, 1000), chargeSlots = rand.Next(0, 100) };
            Config.VacantIndexS = 2;

            for (int i = 0; i < 10; i++)
                customers[i] = new Customer { id = rand.Next(100000000, 999999999), name = names[rand.Next(0, 4)], phone = "05" + rand.Next(10000000, 99999999), longitude = rand.Next(10, 1000), lattitued = rand.Next(10, 1000) };
            Config.VacantIndexC = 10;

            for (int i = 0; i < 10; i++)
                parcels[i] = new Parcel { id = rand.Next(100000000, 999999999), senderId = rand.Next(100000000, 999999999), targetId = rand.Next(100000000, 999999999), weight = (WeightCategories)rand.Next(0, 2), priority = (Priorities)rand.Next(0, 2), droneId = rand.Next(100000000, 999999999), requested = DateTime.Now, scheduled = DateTime.Now, pickedUp = DateTime.Now, delivered = DateTime.Now };
            Config.VacantIndexP = 10;
            Config.ParcelsId = 1000000000;//bigger frome all the ID 
        }
    }
}