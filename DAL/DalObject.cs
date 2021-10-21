using System;
using IDAL.DO;

namespace DalObject
{
    public class DalObject
    {
        public DalObject()
        {
            DataSource.Initialize();
        }

        public static void AddStation(Station newStation)
        {
            if (DataSource.Config.VacantIndexS == DataSource.stations.Length) // if there is no place in array
            {
                Station[] newArrayStations = new Station[DataSource.Config.VacantIndexS * 2]; // increse the place in array double 2
                for (int i = 0; i < DataSource.stations.Length; i++)
                    newArrayStations[i] = DataSource.stations[i]; // copy the old array to new array
                DataSource.stations = newArrayStations;
            }
            DataSource.stations[DataSource.Config.VacantIndexS] = newStation; // insert to the last place in array
            DataSource.Config.VacantIndexS++; // the new spote of the empty index in array
        }

        public static void AddDrone(Drone newDrone)
        {
            if (DataSource.Config.VacantIndexD == DataSource.drones.Length) // if there is no place in array
            {
                Drone[] newArrayDrones = new Drone[DataSource.Config.VacantIndexD * 2]; // increse the place in array double 2
                for (int i = 0; i < DataSource.drones.Length; i++)
                    newArrayDrones[i] = DataSource.drones[i]; // copy the old array to new array
                DataSource.drones = newArrayDrones;
            }
            DataSource.drones[DataSource.Config.VacantIndexD] = newDrone; // insert to the last place in array
            DataSource.Config.VacantIndexD++; // the new spote of the empty index in array
        }

        public static int AddParcel(Parcel newParcel)
        {
            if (DataSource.Config.VacantIndexP == DataSource.parcels.Length) // if there is no place in array
            {
                Parcel[] newArrayParcels = new Parcel[DataSource.Config.VacantIndexP * 2]; // increse the place in array double 2
                for (int i = 0; i < DataSource.parcels.Length; i++)
                    newArrayParcels[i] = DataSource.parcels[i]; // copy the old array to new array
                DataSource.parcels = newArrayParcels;
            }

            DataSource.parcels[DataSource.Config.VacantIndexP] = newParcel; // insert to the last place in array
            DataSource.Config.VacantIndexP++; // the new spote of the empty index in array
            newParcel.id = DataSource.Config.ParcelsId; // insert the Parcels new Id
            int tmp = DataSource.Config.ParcelsId;
            DataSource.Config.ParcelsId++; // new Id for the fautre parce Id
            return tmp; // return the new number created
        }

        public static void AddCustomer(Customer newCustomer)
        {
            if (DataSource.Config.VacantIndexC == DataSource.customers.Length) // if there is no place in array
            {
                Customer[] newArrayCustomers = new Customer[DataSource.Config.VacantIndexC * 2]; // increse the place in array double 2
                for (int i = 0; i < DataSource.customers.Length; i++)
                    newArrayCustomers[i] = DataSource.customers[i]; // copy the old array to new array
                DataSource.customers = newArrayCustomers;
            }
            DataSource.customers[DataSource.Config.VacantIndexC] = newCustomer; // insert to the last place in array
            DataSource.Config.VacantIndexC++;  // the new spote of the empty index in array
        }

        public static Station GetStation(int StationId)
        {
            Station NewStation = new Station();
            for (int i = 0; i < DataSource.Config.VacantIndexS; i++)
            {
                if(DataSource.Stations[i].Id == StationId)
                {
                    NewStation = DataSource.Stations[i];
                }
            }
            return NewStation;
        }

        public static Drone GetDrone(int DroneId)
        {
            Drone NewDrone = new Drone();
            for (int i = 0; i < DataSource.Config.VacantIndexD; i++)
            {
                if (DataSource.Drones[i].Id == DroneId)
                {
                    NewDrone = DataSource.Drones[i];
                }
            }
            return NewDrone;
        }

        public static Parcel GetParcel(int ParcelId)
        {
            Parcel NewParcel = new Parcel();
            for (int i = 0; i < DataSource.Config.VacantIndexP; i++)
            {
                if (DataSource.Parcels[i].Id == ParcelId)
                {
                    NewParcel = DataSource.Parcels[i];
                }
            }
            return NewParcel;
        }

        public static Customer GetCustomer(int CustomerId)
        {
            Customer NewCustomer = new Customer();
            for (int i = 0; i < DataSource.Config.VacantIndexC; i++)
            {
                if (DataSource.Customers[i].Id == CustomerId)
                {
                    NewCustomer = DataSource.Customers[i];
                }
            }
            return NewCustomer;
        }

        public static void PrintStations()//print the list
        {
            for (int i = 0; i < DataSource.Config.VacantIndexS; i++)
                Console.WriteLine(DataSource.Stations[i].ToString());
        }

        public static void PrintDrones()//print the list
        {
            for (int i = 0; i < DataSource.Config.VacantIndexD; i++)
                Console.WriteLine(DataSource.Drones[i].ToString());
        }

        public static void PrintDronesAvailable()//print the list
        {
            for (int i = 0; i < DataSource.Config.VacantIndexD; i++)
                if (DataSource.Drones[i].Status == DroneStatuses.Available)
                    Console.WriteLine(DataSource.Drones[i].ToString());
        }

        public static void PrintParcels()//print the list
        {
            for (int i = 0; i < DataSource.Config.VacantIndexP; i++)
                Console.WriteLine(DataSource.Parcels[i].ToString());
        }

        public static void PrintCustomers()//print the list
        {
            for (int i = 0; i < DataSource.Config.VacantIndexC; i++)
                Console.WriteLine(DataSource.Customers[i].ToString());
        }

        public static void PrintParcelsNoDrones()//print the list
        {
            for (int i = 0; i < DataSource.Config.VacantIndexP; i++)
                if (DataSource.Parcels[i].DroneId == -1)//the id drone is not exist
                    Console.WriteLine(DataSource.Parcels[i].ToString());
        }

        public static void PrintStationsCharge()//print the list
        {
            for (int i = 0; i < DataSource.Config.VacantIndexS; i++)
                if (DataSource.Stations[i].ChargeSlots > 0)
                    Console.WriteLine(DataSource.Stations[i].ToString());
        }

        public static void ConnectParcelToDrone()//Assign a package to a skimmer
        {
            int IdDrone, IdParcel;
            Console.WriteLine("Enter id of parcel to connect:\n");
            PrintParcelsNoDrones();
            int.TryParse(Console.ReadLine(), out IdParcel);
            Parcel P = GetParcel(IdParcel);
            Console.WriteLine("Enter id of drone:\n");
            PrintDronesAvailable();
            int.TryParse(Console.ReadLine(), out IdDrone);
            Drone D = GetDrone(IdDrone);
            P.Requested = DateTime.Now;//The time to create a package for delivery
            D.Status = DroneStatuses.Delivery;
            P.DroneId = D.Id;//conect
            P.Scheduled = DateTime.Now;
        }

        public static void CollectionParcelByDrone()//Assign a package to a drone
        {
            int IdParcel;
            Console.WriteLine("Enter id of parcel to PickedUp:\n");
            for (int i = 0; i < DataSource.Config.VacantIndexP; i++)
                if (DataSource.Parcels[i].DroneId != -1)//the parcel was connected
                    Console.WriteLine(DataSource.Parcels[i].ToString());
            int.TryParse(Console.ReadLine(), out IdParcel);
            Parcel P = GetParcel(IdParcel);
            P.PickedUp = DateTime.Now;
        }

        public static void SupplyParcelToCustomer()
        {
            int IdParcel;
            Console.WriteLine("Enter id of parcel to delivered:\n");
            for (int i = 0; i < DataSource.Config.VacantIndexP; i++)
                if (DataSource.Parcels[i].PickedUp != null)//the parcel was pickup
                    Console.WriteLine(DataSource.Parcels[i].ToString());
            int.TryParse(Console.ReadLine(), out IdParcel);
            Parcel P = GetParcel(IdParcel);
            P.Delivered = DateTime.Now;
        }

        public static void SendDroneToDroneCharge(int IdStation)
        {
            int IdDrone;
            Console.WriteLine("Enter id of drone:\n");
            PrintDrones();
            int.TryParse(Console.ReadLine(), out IdDrone);
            Drone D = GetDrone(IdDrone);
            D.Status = DroneStatuses.Maintenance;
            Station S = GetStation(IdStation);
            S.ChargeSlots--;
            DataSource.DroneCharges[DataSource.Config.VacantIndexDC].DroneId = D.Id;
            DataSource.DroneCharges[DataSource.Config.VacantIndexDC].Stationld = S.Id;
            DataSource.Config.VacantIndexDC++;
            D.Battry = 100;
        }

        public static void ReleaseDroneFromDroneCharge()
        {
            int IdStation, IdDrone;
            Console.WriteLine("Enter id of Station and Drone to relese:\n");
            for (int i = 0; i < DataSource.Config.VacantIndexDC; i++)
                Console.WriteLine(DataSource.DroneCharges[i].ToString());
            int.TryParse(Console.ReadLine(), out IdStation);
            Station S = GetStation(IdStation);
            int.TryParse(Console.ReadLine(), out IdDrone);
            Drone D = GetDrone(IdDrone);
            S.ChargeSlots++;
            D.Status = DroneStatuses.Available;
            DroneCharge[] NewDroneCharges = new DroneCharge[100];
            for (int i = 0; i < DataSource.DroneCharges.Length; i++)
            {
                if (DataSource.DroneCharges[i].DroneId == D.Id && DataSource.DroneCharges[i].Stationld == S.Id)
                {
                    for (int j = 0, k = 0; j < DataSource.DroneCharges.Length; j++, k++)//to remove the elemnt
                    {
                        if (j != i)//copy all the array befor I
                            NewDroneCharges[k] = DataSource.DroneCharges[j];
                        else//keep the index of I to remove the elemnt
                            k--;
                    }
                }
            }
            DataSource.DroneCharges = NewDroneCharges;
            DataSource.Config.VacantIndexDC--;
            S.ChargeSlots++;
        }
    }
}



