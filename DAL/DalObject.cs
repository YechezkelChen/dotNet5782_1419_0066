using System;
using IDAL.DO;

namespace DalObjects
{
    public class DalObjects
    {
        public void DalObject()
        {
            DataSource.Initialize();
        }

        public static void AddStation()
        {
            Station NewStation;
            Console.WriteLine("Enter Id Station: ");
            int.TryParse(Console.ReadLine(), out NewStation.Id);
            Console.WriteLine("Enter Name Station: ");
            int.TryParse(Console.ReadLine(), out NewStation.Name);
            Console.WriteLine("Enter Longitude Station: ");
            int.TryParse(Console.ReadLine(), out NewStation.Longitude);
            Console.WriteLine("Enter Lattitued Station: ");
            int.TryParse(Console.ReadLine(), out NewStation.Lattitued);
            Console.WriteLine("Enter ChargeSlots Station: ");
            int.TryParse(Console.ReadLine(), out NewStation.ChargeSlots);

            if (DalObject.DataSource.Config.VacantIndexS == sizeof(IDAL.DO.Station[] DataSource.Stations)) // if there is no place in array
            {
                IDAL.DO.Station[] NewArrayStations = new IDAL.DO.Station[DalObject.DataSource.Config.VacantIndexS * 2]; // increse the place in array double 2
                for (int i = 0; i < sizeof(IDAL.DO.Station[] DataSource.Stations) ; i++)
                    IDAL.DO.Station[] NewArrayStations[i] = IDAL.DO.Station[] DataSource.Stations[i]; // copy the old array to new array
                IDAL.DO.Station[] DataSource.Stations = IDAL.DO.Station[] NewArrayStations;
            }
            IDAL.DO.Station[] DataSource.Stations[DalObject.DataSource.Config.VacantIndexS] = NewStation; // insert to the last place in array
            DalObject.DataSource.Config.VacantIndexS++; // the new spote of the empty index in array
            throw new NotImplementedException();
        }

        public static void AddDrone()
        {
            IDAL.DO.Drone NewDrone;
            Console.WriteLine("Enter Id Drone: ");
            int.TryParse(Console.ReadLine(), out NewDrone.Id);
            Console.WriteLine("Enter Model Drone: ");
            int.TryParse(Console.ReadLine(), out NewDrone.Model);
            Console.WriteLine("Enter MaxWeight Drone: ");
            int.TryParse(Console.ReadLine(), out NewDrone.MaxWeight);
            Console.WriteLine("Enter Status Drone: ");
            int.TryParse(Console.ReadLine(), out NewDrone.Status);
            Console.WriteLine("Enter Battry Drone: ");
            int.TryParse(Console.ReadLine(), out NewDrone.Battry);

            if (DalObject.DataSource.Config.VacantIndexD == sizeof(IDAL.DO.Drone[] DataSource.Drones)) // if there is no place in array
            {
                IDAL.DO.Drone[] NewArrayDrones = new IDAL.DO.Drone[DalObject.DataSource.Config.VacantIndexD * 2]; // increse the place in array double 2
                for (int i = 0; i < sizeof(IDAL.DO.Drone[] DataSource.Drones) ; i++)
                    IDAL.DO.Drone[] NewArrayDrones[i] = IDAL.DO.Drone[] DataSource.Drones[i]; // copy the old array to new array
                IDAL.DO.Drone[] DataSource.Drones = IDAL.DO.Drone[] NewArrayDrones;
            }
            IDAL.DO.Drone[] DataSource.Drones[DalObject.DataSource.Config.VacantIndexD] = NewDrone; // insert to the last place in array
            DalObject.DataSource.Config.VacantIndexD++; // the new spote of the empty index in array
            throw new NotImplementedException();
        }

        public static int AddParcel()
        {
            IDAL.DO.Parcel NewParcel;
            Console.WriteLine("Enter Id Parcel: ");
            int.TryParse(Console.ReadLine(), out NewParcel.Id);
            Console.WriteLine("Enter Sender Id Parcel: ");
            int.TryParse(Console.ReadLine(), out NewParcel.SenderId);
            Console.WriteLine("Enter Target Id Parcel: ");
            int.TryParse(Console.ReadLine(), out NewParcel.TargetId);
            Console.WriteLine("Enter Weight Parcel: ");
            int.TryParse(Console.ReadLine(), out NewParcel.Weight);
            Console.WriteLine("Enter Priority Parcel: ");
            int.TryParse(Console.ReadLine(), out NewParcel.Priority);
            Console.WriteLine("Enter Drone Id Parcel: ");
            int.TryParse(Console.ReadLine(), out NewParcel.DroneId);
            Console.WriteLine("Enter Requested Time Parcel: ");
            int.TryParse(Console.ReadLine(), out NewParcel.Requested);
            Console.WriteLine("Enter Scheduled Time Parcel: ");
            int.TryParse(Console.ReadLine(), out NewParcel.Scheduled);
            Console.WriteLine("Enter Picked Up Time Parcel: ");
            int.TryParse(Console.ReadLine(), out NewParcel.PickedUp);
            Console.WriteLine("Enter Delivered Time Parcel: ");
            int.TryParse(Console.ReadLine(), out NewParcel.Delivered);

            if (DalObject.DataSource.Config.VacantIndexP == sizeof(IDAL.DO.Customer[] DataSource.Parcels) // if there is no place in array
            {
                IDAL.DO.Parcel[] NewArrayParcels = new IDAL.DO.Parcel[DalObject.DataSource.Config.VacantIndexP * 2]; // increse the place in array double 2
                for (int i = 0; i < sizeof(IDAL.DO.Parcel[] DataSource.Parcels) ; i++)
                    IDAL.DO.Parcel[] NewArrayParcels[i] = IDAL.DO.Parcel[] DataSource.Parcels[i]; // copy the old array to new array
                IDAL.DO.Parcel[] DataSource.Parcels = IDAL.DO.Parcel[] NewArrayParcels;
            }
            NewParcel.Id = DalObject.DataSource.Config.ParcelsId; // insert the Parcels new Id
            IDAL.DO.Parcel[] DataSource.Parcels[DalObject.DataSource.Config.VacantIndexP] = NewParcel; // insert to the last place in array
            DalObject.DataSource.Config.VacantIndexP++; // the new spote of the empty index in array
            int tmp = ParcelsId;
            DalObject.DataSource.Config.ParcelsId++; // new Id for the fautre parce Id
            return tmp; // return the new number created
            throw new NotImplementedException();
        }

        public static void AddCustomer()
        {
            IDAL.DO.Customer NewCustomer;
            Console.WriteLine("Enter Id Customer: ");
            int.TryParse(Console.ReadLine(), out NewCustomer.Id);
            Console.WriteLine("Enter Name Customer: ");
            int.TryParse(Console.ReadLine(), out NewCustomer.Name);
            Console.WriteLine("Enter Phone Customer: ");
            int.TryParse(Console.ReadLine(), out NewCustomer.Phone);
            Console.WriteLine("Enter Longitude Customer: ");
            int.TryParse(Console.ReadLine(), out NewCustomer.Longitude);
            Console.WriteLine("Enter Lattitued Customer: ");
            int.TryParse(Console.ReadLine(), out NewCustomer.Lattitued);

            if (DalObject.DataSource.Config.VacantIndexC == sizeof(IDAL.DO.Customer[] DataSource.Customers)) // if there is no place in array
            {
                IDAL.DO.Customer[] NewArrayCustomers = new IDAL.DO.Customer[DalObject.DataSource.Config.VacantIndexC * 2]; // increse the place in array double 2
                for (int i = 0; i < sizeof(IDAL.DO.Customer[] DataSource.Customers) ; i++)
                    IDAL.DO.Customer[] NewArrayCustomers[i] = IDAL.DO.Customer[] DataSource.Customers[i]; // copy the old array to new array
                IDAL.DO.Customer[] DataSource.Customers = IDAL.DO.Customer[] NewArrayCustomers;
            }
            IDAL.DO.Customer[] DataSource.Customers[DalObject.DataSource.Config.VacantIndexC] = NewCustomer; // insert to the last place in array
            DalObject.DataSource.Config.VacantIndexC++; // the new spote of the empty index in array
            throw new NotImplementedException();
        }

        public static IDAL.DO.Station GetStation(int StationId)
        {
            return IDAL.DO.Station[StationId];
        }

        public static IDAL.DO.Drone GetDrone(int DroneId)
        {
            return IDAL.DO.Drone[DroneId];
        }

        public static IDAL.DO.Parcel GetParcel(int ParcelId)
        {
            return IDAL.DO.Parcel[ParcelId];
        }

        public static IDAL.DO.Customer GetCustomer(int CustomerId)
        {
            return IDAL.DO.Customer[CustomerId];
        }
    }
}


