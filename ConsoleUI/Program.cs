using System;
using IDAL.DO;
using DalObject;
using System.Collections.Generic;
using System.Linq;
using IDAL;

namespace ConsoleUI
{
    class Program
    {
        public enum Option { Add = 1, Update, View, ListView, Exit };
        public enum EntityOption { Station = 1, Drone, Customer, Parcel, Exit };

        public enum OptionListView { ListStations = 1, ListDrones, ListCustomers, ListParcels, ListParcelsNoDrones, ListStationsCharge, Exit };

        public enum OptionUpdate { ConnectParcelToDrone = 1, CollectionParcelByDrone, SupplyParcelToCustomer, SendDroneToDroneCharge, ReleaseDroneFromDroneCharge, Exit };

        static void Main(string[] args)
        { 
            //IDal dal = new DalObject.DalObject();
            DalObject.DalObject dal  = new DalObject.DalObject();//for the initialize
            int c, idDrone, idParcel, idStation;
            Option op;
            EntityOption ep;
            OptionListView olv;
            OptionUpdate ou;
            do
	        {
                Console.WriteLine("\nHELLO\n" + "Choose one of the following:\n" + "1: Add\n" + "2: Update\n" + "3: View\n" + "4: List View\n" + "5: Exit\n");
                int.TryParse(Console.ReadLine(), out c);
                op = (Option)c;
                try
                {
                    switch (op)
                    {
                        case Option.Add:
                            Console.WriteLine("Choose one of the entity:\n" + "1: Station\n" + "2: Drone\n" + "3: Customer\n" + "4: Parcel\n" + "5: Exit\n");
                            int.TryParse(Console.ReadLine(), out c);
                            ep = (EntityOption)c;
                            switch (ep)
                            {
                                case EntityOption.Station:
                                    Station newStation = InputStation();
                                    dal.AddStation(newStation);
                                    break;
                                case EntityOption.Drone:
                                    Drone newDrone = InputDrone();
                                    dal.AddDrone(newDrone);
                                    break;
                                case EntityOption.Customer:
                                    Customer newCustomer = InputCustomer();
                                    dal.AddCustomer(newCustomer);
                                    break;
                                case EntityOption.Parcel:
                                    Parcel newParcel = InputParcel();
                                    dal.AddParcel(newParcel);
                                    break;
                                case EntityOption.Exit:
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case Option.Update:
                            Console.WriteLine("Choose one of to update:\n" + "1: Connect Parcel To Drone:\n" + "2: Collection Parcel By Drone\n" + "3: Supply Parcel To Customer\n" + "4: Send Drone To Drone Charge\n" + "5: Release Drone From Drone Charge\n" + "6: Exit\n");
                            int.TryParse(Console.ReadLine(), out c);
                            ou = (OptionUpdate)c;
                            switch (ou)
                            {
                                case OptionUpdate.ConnectParcelToDrone:
                                    Console.WriteLine("Enter Id of parcel to connect:\n");
                                    PrintParcelsNoDrones(dal);
                                    int.TryParse(Console.ReadLine(), out idParcel);
                                    Parcel parcel = dal.GetParcel(idParcel);
                                    Console.WriteLine("Enter Id of drone:\n");
                                    PrintDronesAvailable(dal);
                                    int.TryParse(Console.ReadLine(), out idDrone);
                                    Drone drone = dal.GetDrone(idDrone);
                                    dal.ConnectParcelToDrone(parcel, drone);
                                    break;
                                case OptionUpdate.CollectionParcelByDrone:
                                    Console.WriteLine("Enter Id of parcel to PickedUp:\n");
                                    PrintParcelsWithNoAssign(dal);
                                    int.TryParse(Console.ReadLine(), out idParcel);
                                    Parcel helpParcel = dal.GetParcel(idParcel);
                                    dal.CollectionParcelByDrone(helpParcel);
                                    break;
                                case OptionUpdate.SupplyParcelToCustomer:
                                    Console.WriteLine("Enter Id of parcel to delivered:\n");
                                    PrintParcelsPickedUp(dal);
                                    int.TryParse(Console.ReadLine(), out idParcel);
                                    Parcel p = dal.GetParcel(idParcel);
                                    dal.SupplyParcelToCustomer(p);
                                    break;
                                case OptionUpdate.SendDroneToDroneCharge:
                                    Console.WriteLine("Enter the Id station:\n");
                                    PrintStationsCharge(dal);
                                    int.TryParse(Console.ReadLine(), out idStation);
                                    Station helpStation = dal.GetStation(idStation);
                                    Console.WriteLine("Enter Id of drone:\n");
                                    PrintDrones(dal);
                                    int.TryParse(Console.ReadLine(), out idDrone);
                                    Drone helpDrone = dal.GetDrone(idDrone);
                                    dal.SendDroneToDroneCharge(helpStation, helpDrone);
                                    break;
                                case OptionUpdate.ReleaseDroneFromDroneCharge:
                                    Console.WriteLine("Enter Id of Station with place to charge:\n");
                                    PrintStationsCharge(dal);
                                    int.TryParse(Console.ReadLine(), out idStation);
                                    Station chargeStation = dal.GetStation(idStation);
                                    Console.WriteLine("Enter Id of Drone to relese:\n");
                                    PrintDronesCharge(dal);
                                    int.TryParse(Console.ReadLine(), out idDrone);
                                    Drone releaseDrone = dal.GetDrone(idDrone);
                                    dal.ReleaseDroneFromDroneCharge(chargeStation, releaseDrone);
                                    break;
                                case OptionUpdate.Exit:
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case Option.View:
                            Console.WriteLine("Choose one of the entity:\n" + "1: Station\n" + "2: Drone\n" + "3: Customer\n" + "4: Parcel\n" + "5: Exit\n");
                            int.TryParse(Console.ReadLine(), out c);
                            ep = (EntityOption)c;
                            int myId;
                            Console.WriteLine("Enter Id of the entity:\n");
                            int.TryParse(Console.ReadLine(), out myId);
                            switch (ep)
                            {
                                case EntityOption.Station:
                                    Console.WriteLine(dal.GetStation(myId).ToString());
                                    break;
                                case EntityOption.Drone:
                                    Console.WriteLine(dal.GetDrone(myId).ToString());
                                    break;
                                case EntityOption.Customer:
                                    Console.WriteLine(dal.GetCustomer(myId).ToString());
                                    break;
                                case EntityOption.Parcel:
                                    Console.WriteLine(dal.GetParcel(myId).ToString());
                                    break;
                                case EntityOption.Exit:
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case Option.ListView:
                            Console.WriteLine("Choose one of the entity:\n" + "1: List Stations\n" + "2: List Drones\n" + "3: List Customers\n" + "4: List Parcels\n" + "5: List Parcels No Drones\n" + "6: List Stations Charge\n" + "7:Exit\n");
                            int.TryParse(Console.ReadLine(), out c);
                            olv = (OptionListView)c;
                            switch (olv)
                            {
                                case OptionListView.ListStations:
                                    PrintStations(dal);
                                    break;
                                case OptionListView.ListDrones:
                                    PrintDrones(dal);
                                    break;
                                case OptionListView.ListCustomers:
                                    PrintCustomers(dal);
                                    break;
                                case OptionListView.ListParcels:
                                    PrintParcels(dal);
                                    break;
                                case OptionListView.ListParcelsNoDrones:
                                    PrintParcelsNoDrones(dal);
                                    break;
                                case OptionListView.ListStationsCharge:
                                    PrintStationsCharge(dal);
                                    break;
                                case OptionListView.Exit:
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case Option.Exit:
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)//generally exception for all the options
                {
                    Console.WriteLine(ex);
                }
	        } while (op != Option.Exit);
        }

        /// <summary>
        /// read fron the user station to insert to list
        /// </summary>
        /// <returns></no returns, just read from user>
        public static Station InputStation()
        {
            int num1;
            double num2;
            Station NewStation = new Station();
            do
            {
                Console.WriteLine("Enter Id Station: ");
            } while (!int.TryParse(Console.ReadLine(), out num1));
            NewStation.id = num1;

            do
            {
                Console.WriteLine("Enter Name Station: ");

            } while (!int.TryParse(Console.ReadLine(), out num1));
            NewStation.name = num1;

            do
            {
                Console.WriteLine("Enter Longitude Station: ");

            } while (!double.TryParse(Console.ReadLine(), out num2));
            NewStation.longitude = num2;

            do
            {
                Console.WriteLine("Enter Lattitued Station: ");

            } while (!double.TryParse(Console.ReadLine(), out num2));
            NewStation.lattitued = num2;

            do
            {
                Console.WriteLine("Enter ChargeSlots Station: ");

            } while (!int.TryParse(Console.ReadLine(), out num1));
            NewStation.chargeSlots = num1;

            return NewStation;
        }

        /// <summary>
        /// read fron the user drone to insert to list
        /// </summary>
        /// <returns></no returns, just read from user>
        public static Drone InputDrone()
        {
            int num;
            Drone NewDrone = new Drone();

            do
            {
                Console.WriteLine("Enter Id Drone: ");
            } while (!int.TryParse(Console.ReadLine(), out num));
            NewDrone.Id = num;

            Console.WriteLine("Enter Model Drone: ");
            NewDrone.model = Console.ReadLine();

            do
            {
                Console.WriteLine("Enter MaxWeight Drone:\n" + "1: Light\n" + "2: Medium\n" + "3: Heavy\n");
                int.TryParse(Console.ReadLine(), out num);
            } while (num != 1 && num != 2 && num != 3);
            switch (num)
            {
                case 1:
                    NewDrone.maxWeight = WeightCategories.Light;
                    break;
                case 2:
                    NewDrone.maxWeight = WeightCategories.Medium;
                    break;
                case 3:
                    NewDrone.maxWeight = WeightCategories.Heavy;
                    break;
                default:
                    break;
            }

            return NewDrone;
        }

        /// <summary>
        /// read fron the user customer to insert to list
        /// </summary>
        /// <returns></no returns, just read from user>
        public static Customer InputCustomer()
        {
            int num;
            double d;
            Customer NewCustomer = new Customer();
            do
            {
                Console.WriteLine("Enter Id Customer: ");
            } while (!int.TryParse(Console.ReadLine(), out num));
            NewCustomer.id = num;

            Console.WriteLine("Enter Name Customer: ");
            NewCustomer.name = Console.ReadLine();

            Console.WriteLine("Enter Phone Customer: ");
            NewCustomer.phone = Console.ReadLine();

            do
            {
                Console.WriteLine("Enter Longitude Customer: ");
            } while (!double.TryParse(Console.ReadLine(), out d));
            NewCustomer.longitude = d;

            do
            {
                Console.WriteLine("Enter Lattitued Customer: ");
            } while (!double.TryParse(Console.ReadLine(), out d));
            NewCustomer.lattitued = d;

            return NewCustomer;
        }

        /// <summary>
        /// read fron the user parcel to insert to list
        /// </summary>
        /// <returns></no returns, just read from user>
        public static Parcel InputParcel()
        {
            int num;
            Parcel NewParcel = new Parcel();
            do
            {
                Console.WriteLine("Enter Id Parcel: ");

            } while (!int.TryParse(Console.ReadLine(), out num));
            NewParcel.Id = num;

            do
            {
                Console.WriteLine("Enter Sender Id Parcel: ");
            } while (!int.TryParse(Console.ReadLine(), out num));
            NewParcel.SenderId = num;

            do
            {
                Console.WriteLine("Enter Target Id Parcel: ");
            } while (!int.TryParse(Console.ReadLine(), out num));
            NewParcel.TargetId = num;

            do
            {
                Console.WriteLine("Enter Weight Parcel:\n" + "1: Light\n" + "2: Medium\n" + "3: Heavy\n");
                int.TryParse(Console.ReadLine(), out num);
            } while (num != 1 && num != 2 && num != 3);
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
            } while (num != 1 && num != 2 && num != 3);
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

            NewParcel.DroneId = 0;
            NewParcel.Requested = DateTime.MinValue;
            NewParcel.Scheduled = DateTime.MinValue;
            NewParcel.PickedUp = DateTime.MinValue;
            NewParcel.Delivered = DateTime.MinValue;

            return NewParcel;
        }

        /// <summary>
        /// print all the list of stations
        /// </summary>
        /// <returns></no returns, just print>
        public static void PrintStations(DalObject.DalObject dal)//print the list
        {
            foreach (Station elementStation in dal.GetStations())
                Console.WriteLine(elementStation.ToString());
        }

        /// <summary>
        /// print all the list of drones
        /// </summary>
        /// <returns></no returns, just print>
        public static void PrintDrones(DalObject.DalObject dal)//print the list
        {
            foreach (Drone elementDrone in dal.GetDrones()) 
                Console.WriteLine(elementDrone.ToString());
        }

        /// <summary>
        /// print all the list of Drones Available
        /// </summary>
        /// <returns></no returns, just print>
        public static void PrintDronesAvailable(DalObject.DalObject dal)//print the list
        {
            IEnumerable<Drone> newDrones = dal.GetDrones();
            foreach (Drone elementDrone in newDrones)
            {
                
            }

            //for (int i = 0; i < newDrones.Length; i++)
            //    if (newDrones[i].status == DroneStatuses.Available)
            //        Console.WriteLine(newDrones[i].ToString());
        }

        /// <summary>
        /// print all the list of Drones Charge
        /// </summary>
        /// <returns></no returns, just print>
        public static void PrintDronesCharge(DalObject.DalObject dal)//print the list
        {
            foreach (DroneCharge elementDroneCharge in dal.GetDronesCharge())
                Console.WriteLine(elementDroneCharge.ToString());
        }

        /// <summary>
        /// print all the list of customers
        /// </summary>
        /// <returns></no returns, just print>
        public static void PrintCustomers(DalObject.DalObject dal)//print the list
        {
            foreach (Customer elementCustomer in dal.GetCustomers())
                Console.WriteLine(elementCustomer.ToString());
        }

        /// <summary>
        /// print all the list of parcels
        /// </summary>
        /// <returns></no returns, just print>
        public static void PrintParcels(DalObject.DalObject dal)//print the list
        {
            foreach (Parcel elementParcel in dal.GetParcels())
                Console.WriteLine(elementParcel.ToString());
        }

        /// <summary>
        /// print all the list of Parcels With No Assign 
        /// </summary>
        /// <returns></no returns, just print>
        public static void PrintParcelsWithNoAssign(DalObject.DalObject dal)//print the list
        {
            foreach (Parcel elementParcel in dal.GetParcels())
                if (elementParcel.DroneId != -1)//the parcel wasnt connected
                    Console.WriteLine(elementParcel.ToString());
        }

        /// <summary>
        /// print all the list of Parcels Picked Up
        /// </summary>
        /// <returns></no returns, just print>
        public static void PrintParcelsPickedUp(DalObject.DalObject dal)//print the list
        {
            foreach (Parcel elementParcel in dal.GetParcels())
                if (elementParcel.PickedUp != DateTime.MinValue)//the parcel was pickup
                    Console.WriteLine(elementParcel.ToString());
        }

        /// <summary>
        /// print all the list of Parcels No Drones
        /// </summary>
        /// <returns></no returns, just print>
        public static void PrintParcelsNoDrones(DalObject.DalObject dal)//print the list
        {
            foreach (Parcel elementParcel in dal.GetParcels())
                if (elementParcel.DroneId == -1)//the Id drone is not exist
                    Console.WriteLine(elementParcel.ToString());
        }

        /// <summary>
        /// print all the list of Stations Charge
        /// </summary>
        /// <returns></no returns, just print>
        public static void PrintStationsCharge(DalObject.DalObject dal)//print the list
        {
            foreach (Station elementStation in dal.GetStations())
                if (elementStation.chargeSlots > 0)
                    Console.WriteLine(elementStation.ToString());
        }
    }
}
