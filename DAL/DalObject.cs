﻿using System;
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

            if (DataSource.Config.VacantIndexS == DataSource.Stations.Length) // if there is no place in array
            {
                Station[] NewArrayStations = new Station[DataSource.Config.VacantIndexS * 2]; // increse the place in array double 2
                for (int i = 0; i < DataSource.Stations.Length; i++)
                    NewArrayStations[i] = DataSource.Stations[i]; // copy the old array to new array
                DataSource.Stations = NewArrayStations;
            }
            DataSource.Stations[DataSource.Config.VacantIndexS] = NewStation; // insert to the last place in array
            DataSource.Config.VacantIndexS++; // the new spote of the empty index in array
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

            if (DataSource.Config.VacantIndexD == DataSource.Drones.Length) // if there is no place in array
            {
                Drone[] NewArrayDrones = new Drone[DataSource.Config.VacantIndexD * 2]; // increse the place in array double 2
                for (int i = 0; i < DataSource.Drones.Length; i++)
                    NewArrayDrones[i] = DataSource.Drones[i]; // copy the old array to new array
                DataSource.Drones = NewArrayDrones;
            }
            DataSource.Drones[DataSource.Config.VacantIndexD] = NewDrone; // insert to the last place in array
            DataSource.Config.VacantIndexD++; // the new spote of the empty index in array
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

            if (DataSource.Config.VacantIndexP == DataSource.Parcels.Length) // if there is no place in array
            {
                Parcel[] NewArrayParcels = new Parcel[DataSource.Config.VacantIndexP * 2]; // increse the place in array double 2
                for (int i = 0; i < DataSource.Parcels.Length; i++)
                    NewArrayParcels[i] = DataSource.Parcels[i]; // copy the old array to new array
                DataSource.Parcels = NewArrayParcels;
            }

            DataSource.Parcels[DataSource.Config.VacantIndexP] = NewParcel; // insert to the last place in array
            DataSource.Config.VacantIndexP++; // the new spote of the empty index in array
            NewParcel.Id = DataSource.Config.ParcelsId; // insert the Parcels new Id
            int tmp = DataSource.Config.ParcelsId;
            DataSource.Config.ParcelsId++; // new Id for the fautre parce Id
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

            if (DataSource.Config.VacantIndexC == DataSource.Customers.Length) // if there is no place in array
            {
                Customer[] NewArrayCustomers = new Customer[DataSource.Config.VacantIndexC * 2]; // increse the place in array double 2
                for (int i = 0; i < DataSource.Customers.Length; i++)
                    NewArrayCustomers[i] = DataSource.Customers[i]; // copy the old array to new array
                DataSource.Customers = NewArrayCustomers;
            }
            DataSource.Customers[DataSource.Config.VacantIndexC] = NewCustomer; // insert to the last place in array
            DataSource.Config.VacantIndexD++; // the new spote of the empty index in array
            throw new NotImplementedException();
        }

        public static Station GetStation(int StationId)
        {
            for (int i = 0; i < DataSource.Config.VacantIndexS; i++)
            {
                if(DataSource.Stations[i].Id == StationId)
                {
                    return DataSource.Stations[i];
                }
            }
            throw new NotImplementedException();
        }

        public static Drone GetDrone(int DroneId)
        {
            for (int i = 0; i < DataSource.Config.VacantIndexD; i++)
            {
                if (DataSource.Drones[i].Id == DroneId)
                {
                    return DataSource.Drones[i];
                }
            }
            throw new NotImplementedException();
        }

        public static Parcel GetParcel(int ParcelId)
        {
            for (int i = 0; i < DataSource.Config.VacantIndexP; i++)
            {
                if (DataSource.Parcels[i].Id == ParcelId)
                {
                    return DataSource.Parcels[i];
                }
            }
            throw new NotImplementedException();
        }

        public static Customer GetCustomer(int CustomerId)
        {
            for (int i = 0; i < DataSource.Config.VacantIndexC; i++)
            {
                if (DataSource.Customers[i].Id == CustomerId)
                {
                    return DataSource.Customers[i];
                }
            }
            throw new NotImplementedException();
        }

        public static void printStations()//print the list
        {
            for (int i = 0; i < DataSource.Config.VacantIndexS; i++)
                DataSource.Stations[i].ToString();
        }

        public static void printDrones()//print the list
        {
            for (int i = 0; i < DataSource.Config.VacantIndexD; i++)
                DataSource.Drones[i].ToString();
        }

        public static void printParcels()//print the list
        {
            for (int i = 0; i < DataSource.Config.VacantIndexP; i++)
                DataSource.Parcels[i].ToString();
        }

        public static void printCustomers()//print the list
        {
            for (int i = 0; i < DataSource.Config.VacantIndexC; i++)
                DataSource.Customers[i].ToString();
        }

        public static void printParcelsNoDrones()//print the list
        {
            for (int i = 0; i < DataSource.Config.VacantIndexP; i++)
                if (DataSource.Parcels[i].DroneId <= 0)//the id drone is not exist
                    DataSource.Parcels[i].ToString();
        }

        public static void printStationsCharge()//print the list
        {
            for (int i = 0; i < DataSource.Config.VacantIndexS; i++)
                if (DataSource.Stations[i].ChargeSlots > 0)
                    DataSource.Stations[i].ToString();
        }

        public static void ConnectParcelToDrone(Parcel P)//Assign a package to a skimmer
        {
            P.Requested = DateTime.Now;//The time to create a package for delivery
            for (int i = 0; i < DataSource.Config.VacantIndexD; i++)
            {
                if(DataSource.Drones[i].Status==DroneStatuses.Available)
                {
                    DataSource.Drones[i].Status = DroneStatuses.Delivery;
                    P.DroneId = DataSource.Drones[i].Id;//conect
                    P.Scheduled = DateTime.Now;
                }
            }
        }

        public static void PackageCollection(Parcel P)//Assign a package to a skimmer
        {
            for (int i = 0; i < DataSource.Config.VacantIndexD; i++)
            {
                if (DataSource.Drones[i].Status == DroneStatuses.Delivery)
                {
                    if (P.DroneId == DataSource.Drones[i].Id)
                    {
                        P.PickedUp = DateTime.Now;
                        break;
                    }
                }
            }

        }

        public static void PackageDelivery(Parcel P)
        {
            P.Delivered = DateTime.Now;
        }

        public static void SendingDroneToCharge(Drone D,Station S)
        {
            D.Status = DroneStatuses.Maintenance;
            S.ChargeSlots--;
            DataSource.DroneCharges[DataSource.Config.VacantIndexDC].DroneId = D.Id;
            DataSource.DroneCharges[DataSource.Config.VacantIndexDC].Stationld = S.Id;
            DataSource.Config.VacantIndexDC++;
            D.Battry = 100;
        }

        public static void ReleaseDrone(Drone D,Station S)
        {
            DroneCharge[] NewDroneCharges = new DroneCharge[100];
            if(D.Battry==100)
            {
                for (int i = 0; i < DataSource.DroneCharges.Length; i++)
                {
                    if (DataSource.DroneCharges[i].DroneId==D.Id && DataSource.DroneCharges[i].Stationld==S.Id)
                    {
                        for (int j = 0,k=0; j < DataSource.DroneCharges.Length; j++,k++)//to remove the elemnt
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

}


