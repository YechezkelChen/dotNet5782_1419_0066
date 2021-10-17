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

        internal class Config
        {
            internal static int VacantIndexD = 0;//for drone
            internal static int VacantIndexS = 0;//for station
            internal static int VacantIndexC = 0;//for Customer
            internal static int VacantIndexP = 0;//for Parcel
            internal static int ParcelsId = 0;
        }

        public static void Initialize()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            Drones[0] = new Drone { Id = rand.Next(100000000, 999999999), Model = "a", MaxWeight = WeightCategories.Light, Status = DroneStatuses.Available, Battry = 100 };
            Drones[1] = new Drone { Id = rand.Next(100000000, 999999999), Model = "b", MaxWeight = WeightCategories.Medium, Status = DroneStatuses.Available, Battry = 90 };
            Drones[2] = new Drone { Id = rand.Next(100000000, 999999999), Model = "c", MaxWeight = WeightCategories.Heavy, Status = DroneStatuses.Available, Battry = 80 };
            Drones[3] = new Drone { Id = rand.Next(100000000, 999999999), Model = "d", MaxWeight = WeightCategories.Light, Status = DroneStatuses.Delivery, Battry = 70 };
            Drones[4] = new Drone { Id = rand.Next(100000000, 999999999), Model = "e", MaxWeight = WeightCategories.Medium, Status = DroneStatuses.Delivery, Battry = 60 };
            Config.VacantIndexD = 5;

            for (int i = 0; i < 2; i++)
                Stations[i] = new Station { Id = rand.Next(100000000, 999999999), Name = i + i, Longitude = rand.Next(10, 1000), Lattitued = rand.Next(10, 1000), ChargeSlots = i + i + i };
            Config.VacantIndexS = 2;

            Customers[0] = new Customer { Id = rand.Next(100000000, 999999999), Name = "a", Phone = "0541234981", Longitude = rand.Next(10, 1000), Lattitued = rand.Next(10, 1000) };
            Customers[1] = new Customer { Id = rand.Next(100000000, 999999999), Name = "b", Phone = "0541234982", Longitude = rand.Next(10, 1000), Lattitued = rand.Next(10, 1000) };
            Customers[2] = new Customer { Id = rand.Next(100000000, 999999999), Name = "c", Phone = "0541234983", Longitude = rand.Next(10, 1000), Lattitued = rand.Next(10, 1000) };
            Customers[3] = new Customer { Id = rand.Next(100000000, 999999999), Name = "d", Phone = "0541234984", Longitude = rand.Next(10, 1000), Lattitued = rand.Next(10, 1000) };
            Customers[4] = new Customer { Id = rand.Next(100000000, 999999999), Name = "e", Phone = "0541234985", Longitude = rand.Next(10, 1000), Lattitued = rand.Next(10, 1000) };
            Customers[5] = new Customer { Id = rand.Next(100000000, 999999999), Name = "f", Phone = "0541234986", Longitude = rand.Next(10, 1000), Lattitued = rand.Next(10, 1000) };
            Customers[6] = new Customer { Id = rand.Next(100000000, 999999999), Name = "g", Phone = "0541234987", Longitude = rand.Next(10, 1000), Lattitued = rand.Next(10, 1000) };
            Customers[7] = new Customer { Id = rand.Next(100000000, 999999999), Name = "h", Phone = "0541234988", Longitude = rand.Next(10, 1000), Lattitued = rand.Next(10, 1000) };
            Customers[8] = new Customer { Id = rand.Next(100000000, 999999999), Name = "i", Phone = "0541234989", Longitude = rand.Next(10, 1000), Lattitued = rand.Next(10, 1000) };
            Customers[9] = new Customer { Id = rand.Next(100000000, 999999999), Name = "g", Phone = "0541234990", Longitude = rand.Next(10, 1000), Lattitued = rand.Next(10, 1000) };
            Config.VacantIndexC = 10;

            Parcels[0] = new Parcel { Id = rand.Next(100000000, 999999999), SenderId = rand.Next(100000000, 999999999), TargetId = rand.Next(100000000, 999999999), Weight = WeightCategories.Light, Priority = Priorities.Normal, DroneId = Drones[0].Id, Requested = DateTime.Now, Scheduled = DateTime.Now, PickedUp = DateTime.Now, Delivered = DateTime.Now };
            Parcels[1] = new Parcel { Id = rand.Next(100000000, 999999999), SenderId = rand.Next(100000000, 999999999), TargetId = rand.Next(100000000, 999999999), Weight = WeightCategories.Medium, Priority = Priorities.Normal, DroneId = Drones[1].Id, Requested = DateTime.Now, Scheduled = DateTime.Now, PickedUp = DateTime.Now, Delivered = DateTime.Now };
            Parcels[2] = new Parcel { Id = rand.Next(100000000, 999999999), SenderId = rand.Next(100000000, 999999999), TargetId = rand.Next(100000000, 999999999), Weight = WeightCategories.Heavy, Priority = Priorities.Normal, DroneId = Drones[2].Id, Requested = DateTime.Now, Scheduled = DateTime.Now, PickedUp = DateTime.Now, Delivered = DateTime.Now };
            Parcels[3] = new Parcel { Id = rand.Next(100000000, 999999999), SenderId = rand.Next(100000000, 999999999), TargetId = rand.Next(100000000, 999999999), Weight = WeightCategories.Light, Priority = Priorities.Fast, DroneId = Drones[3].Id, Requested = DateTime.Now, Scheduled = DateTime.Now, PickedUp = DateTime.Now, Delivered = DateTime.Now };
            Parcels[4] = new Parcel { Id = rand.Next(100000000, 999999999), SenderId = rand.Next(100000000, 999999999), TargetId = rand.Next(100000000, 999999999), Weight = WeightCategories.Medium, Priority = Priorities.Fast, DroneId = Drones[4].Id, Requested = DateTime.Now, Scheduled = DateTime.Now, PickedUp = DateTime.Now, Delivered = DateTime.Now };
            Parcels[5] = new Parcel { Id = rand.Next(100000000, 999999999), SenderId = rand.Next(100000000, 999999999), TargetId = rand.Next(100000000, 999999999), Weight = WeightCategories.Heavy, Priority = Priorities.Fast, DroneId = Drones[0].Id, Requested = DateTime.Now, Scheduled = DateTime.Now, PickedUp = DateTime.Now, Delivered = DateTime.Now };
            Parcels[6] = new Parcel { Id = rand.Next(100000000, 999999999), SenderId = rand.Next(100000000, 999999999), TargetId = rand.Next(100000000, 999999999), Weight = WeightCategories.Light, Priority = Priorities.Normal, DroneId = Drones[1].Id, Requested = DateTime.Now, Scheduled = DateTime.Now, PickedUp = DateTime.Now, Delivered = DateTime.Now };
            Parcels[7] = new Parcel { Id = rand.Next(100000000, 999999999), SenderId = rand.Next(100000000, 999999999), TargetId = rand.Next(100000000, 999999999), Weight = WeightCategories.Heavy, Priority = Priorities.Emergency, DroneId = Drones[2].Id, Requested = DateTime.Now, Scheduled = DateTime.Now, PickedUp = DateTime.Now, Delivered = DateTime.Now };
            Parcels[8] = new Parcel { Id = rand.Next(100000000, 999999999), SenderId = rand.Next(100000000, 999999999), TargetId = rand.Next(100000000, 999999999), Weight = WeightCategories.Medium, Priority = Priorities.Emergency, DroneId = Drones[3].Id, Requested = DateTime.Now, Scheduled = DateTime.Now, PickedUp = DateTime.Now, Delivered = DateTime.Now };
            Parcels[9] = new Parcel { Id = rand.Next(100000000, 999999999), SenderId = rand.Next(100000000, 999999999), TargetId = rand.Next(100000000, 999999999), Weight = WeightCategories.Light, Priority = Priorities.Emergency, DroneId = Drones[4].Id, Requested = DateTime.Now, Scheduled = DateTime.Now, PickedUp = DateTime.Now, Delivered = DateTime.Now };
            Config.VacantIndexP = 10;
            Config.ParcelsId = 11;
        }
    }
}