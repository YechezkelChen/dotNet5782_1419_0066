using System;

namespace DalObjects
{
      internal class DataSource
    {
        IDAL.DO.Drone[] Drones = new IDAL.DO.Drone[10];
        IDAL.DO.Station[] Stations = new IDAL.DO.Station[5];
        IDAL.DO.Customer[] Customers = new IDAL.DO.Customer[100];
        IDAL.DO.Parcel[] Parcels = new IDAL.DO.Parcel[1000];

        internal class Config
        {
            public static int VacantIndexD = 0;//for drone
            public static int VacantIndexS = 0;//for station
            public static int VacantIndexC = 0;//for Customer
            public static int VacantIndexP = 0;//for Parcel
            public static int ParcelsId =0;
        }

        public static void Initialize()
        {
            IDAL.DO.Drone[] DataSource.Drones[0] = { 1111, a, "Light weight", "Available status", 100};
            IDAL.DO.Drone[] DataSource.Drones[1] = { 2222, b, "Medium weight", "Available status", 90};
            IDAL.DO.Drone[] DataSource.Drones[2] = { 3333, c, "Heavy weight", "Available status", 80};
            IDAL.DO.Drone[] DataSource.Drones[3] = { 4444, d, "Light weight", "Delivery status", 70};
            IDAL.DO.Drone[] DataSource.Drones[4] = { 5555, e, "Medium weight", "Delivery status", 60};
            Config.VacantIndexD = 5;
            
            for (int i = 0; i < 2; i++)
                IDAL.DO.Station[] DataSource.Stations[i] = { i, i+i, rand.Next(), rand.Next(), i+i+i}
            Config.VacantIndexS = 2;

            for (int i = 0, char ch = 'a', string ph = "0525552240"; i < 10; i++, ch++, ph[9] = i)
                IDAL.DO.Customer[] DataSource.Customers[i] = { i, ch, ph, rand.Next(), rand.Next()}
            Config.VacantIndexC = 10;

            static Random rand = new Random(DateTime.Now.Millisecond); 
            IDAL.DO.Parcel[] DataSource.Parcels[0] = { 1, 11, 111, "Light weight", "Normal Priority", 1111, rand.Next(), rand.Next(), rand.Next(), rand.Next()};  
            IDAL.DO.Parcel[] DataSource.Parcels[1] = { 2, 22, 222, "Medium weight" ,"Normal Priority", 2222,rand.Next(),rand.Next(),rand.Next(),rand.Next()};
            IDAL.DO.Parcel[] DataSource.Parcels[2] = { 3, 33, 333, "Heavy weight", "Normal Priority", 3333, rand.Next(), rand.Next(), rand.Next(), rand.Next()};
            IDAL.DO.Parcel[] DataSource.Parcels[3] = { 4 , 44, 444, "Light weight" ,"Fast Priority", 4444,rand.Next(),rand.Next(),rand.Next(),rand.Next()};
            IDAL.DO.Parcel[] DataSource.Parcels[4] = { 5, 55, 555, "Medium weight", "Fast Priority", 5555, rand.Next(), rand.Next(), rand.Next(), rand.Next()};
            IDAL.DO.Parcel[] DataSource.Parcels[5] = { 6, 66, 666, "Heavy weight", "Fast Priority", 6666, rand.Next(), rand.Next(), rand.Next(), rand.Next()};
            IDAL.DO.Parcel[] DataSource.Parcels[6] = { 7, 77, 777, "Light weight", "Normal Priority", 7777, rand.Next(), rand.Next(), rand.Next(), rand.Next()};
            IDAL.DO.Parcel[] DataSource.Parcels[7] ={ 8 , 88, 888, "Heavy weight" ,"Emergency Priority", 8888,rand.Next(),rand.Next(),rand.Next(),rand.Next()};
            IDAL.DO.Parcel[] DataSource.Parcels[8] ={ 9 , 99, 999, "Medium weight" ,"Emergency Priority", 9999,rand.Next(),rand.Next(),rand.Next(),rand.Next()};
            IDAL.DO.Parcel[] DataSource.Parcels[9] ={10 , 1010, 1010, "Light weight" ,"Emergency Priority", 4444,rand.Next(),rand.Next(),rand.Next(),rand.Next()};
            Config.VacantIndexP = 10;
            Config.ParcelsId = 11;
        }
    }
}