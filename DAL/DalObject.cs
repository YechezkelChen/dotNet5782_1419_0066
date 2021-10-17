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

        public static void AddStation()
        {
            int num1;
            double num2;
            Station NewStation = new Station();
            Console.WriteLine("Enter Id Station: ");
            int.TryParse(Console.ReadLine(), out num1);
            NewStation.Id = num1;
            Console.WriteLine("Enter Name Station: ");
            int.TryParse(Console.ReadLine(), out num1);
            NewStation.Name = num1;
            Console.WriteLine("Enter Longitude Station: ");
            double.TryParse(Console.ReadLine(), out num2);
            NewStation.Longitude = num2;
            Console.WriteLine("Enter Lattitued Station: ");
            double.TryParse(Console.ReadLine(), out num2);
            NewStation.Lattitued = num2;
            Console.WriteLine("Enter ChargeSlots Station: ");
            int.TryParse(Console.ReadLine(), out num1);
            NewStation.ChargeSlots = num1;

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
            int num;
            double b;
            Drone NewDrone = new Drone();
            Console.WriteLine("Enter Id Drone: ");
            int.TryParse(Console.ReadLine(), out num);
            NewDrone.Id = num;
            Console.WriteLine("Enter Model Drone: ");
            NewDrone.Model = Console.ReadLine();
            do
            {
                Console.WriteLine("Enter MaxWeight Drone:\n" + "1: Light\n" + "2: Medium\n" + "3: Heavy\n");
                int.TryParse(Console.ReadLine(), out num);
            } while (num != 1 || num != 2 || num != 3);
            switch (num)
            {
                case 1:
                    NewDrone.MaxWeight = WeightCategories.Light;
                    break;
                case 2:
                    NewDrone.MaxWeight = WeightCategories.Medium;
                    break;
                case 3:
                    NewDrone.MaxWeight = WeightCategories.Heavy;
                    break;
                default:
                    break;
            }

            do
            {
                Console.WriteLine("Enter Status Drone:\n" + "1: Available\n" + "2: Maintenance\n" + "3: Delivery\n");
                int.TryParse(Console.ReadLine(), out num);
            } while (num != 1 || num != 2 || num != 3);
            switch (num)
            {
                case 1:
                    NewDrone.Status = DroneStatuses.Available;
                    break;
                case 2:
                    NewDrone.Status = DroneStatuses.Maintenance;
                    break;
                case 3:
                    NewDrone.Status = DroneStatuses.Delivery;
                    break;
                default:
                    break;
            }
            Console.WriteLine("Enter Battry Drone: ");
            double.TryParse(Console.ReadLine(), out b);
            NewDrone.Battry = b;

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
            int num;
            DateTime d;
            Parcel NewParcel = new Parcel();
            Console.WriteLine("Enter Id Parcel: ");
            int.TryParse(Console.ReadLine(), out num);
            NewParcel.Id = num;
            Console.WriteLine("Enter Sender Id Parcel: ");
            int.TryParse(Console.ReadLine(), out num);
            NewParcel.SenderId = num;
            Console.WriteLine("Enter Target Id Parcel: ");
            int.TryParse(Console.ReadLine(), out num);
            NewParcel.TargetId = num;
            do
            {
                Console.WriteLine("Enter Weight Parcel:\n" + "1: Light\n" + "2: Medium\n" + "3: Heavy\n");
                int.TryParse(Console.ReadLine(), out num);
            } while (num != 1 || num != 2 || num != 3);
            switch (num)
            {
                case 1:
                    NewParcel.Weight = WeightCategories.Light;
                    break;
                case 2:
                    NewParcel.Weight = WeightCategories.Medium;
                    break;
                case 3:
                    NewParcel.Weight = WeightCategories.Heavy;
                    break;
                default:
                    break;
            }

            do
            {

                Console.WriteLine("Enter Priority Parcel:\n" + "1: Normal\n" + "2: Fast\n" + "3: Emergency\n");
                int.TryParse(Console.ReadLine(), out num);
            } while (num != 1 || num != 2 || num != 3);
            switch (num)
            {
                case 1:
                    NewParcel.Priority = Priorities.Normal;
                    break;
                case 2:
                    NewParcel.Priority = Priorities.Fast;
                    break;
                case 3:
                    NewParcel.Priority = Priorities.Emergency;
                    break;
                default:
                    break;
            }
            Console.WriteLine("Enter Drone Id Parcel: ");
            int.TryParse(Console.ReadLine(), out num);
            NewParcel.DroneId = num;
            Console.WriteLine("Enter Requested Time Parcel: ");
            DateTime.TryParse(Console.ReadLine(), out d);
            NewParcel.Requested = d;
            Console.WriteLine("Enter Scheduled Time Parcel: ");
            DateTime.TryParse(Console.ReadLine(), out d);
            NewParcel.Scheduled = d;
            Console.WriteLine("Enter Picked Up Time Parcel: ");
            DateTime.TryParse(Console.ReadLine(), out d);
            NewParcel.PickedUp = d;
            Console.WriteLine("Enter Delivered Time Parcel: ");
            DateTime.TryParse(Console.ReadLine(), out d);
            NewParcel.Delivered = d;

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
            int num;
            double d;
            Customer NewCustomer = new Customer();
            Console.WriteLine("Enter Id Customer: ");
            int.TryParse(Console.ReadLine(), out num);
            NewCustomer.Id = num;
            Console.WriteLine("Enter Name Customer: ");
            NewCustomer.Name = Console.ReadLine();
            Console.WriteLine("Enter Phone Customer: ");
            NewCustomer.Phone = Console.ReadLine();
            Console.WriteLine("Enter Longitude Customer: ");
            double.TryParse(Console.ReadLine(), out d);
            NewCustomer.Longitude = d;
            Console.WriteLine("Enter Lattitued Customer: ");
            double.TryParse(Console.ReadLine(), out d);
            NewCustomer.Lattitued = d;

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


