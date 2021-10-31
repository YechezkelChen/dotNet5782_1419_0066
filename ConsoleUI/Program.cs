using System;
using IDAL.DO;
using System.Collections.Generic;
using System.Linq;

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
            DalObject.DalObject d = new DalObject.DalObject();//for the initialize
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
                                DalObject.DalObject.AddStation(newStation);
                                break;
                            case EntityOption.Drone:
                                Drone newDrone = InputDrone();
                                DalObject.DalObject.AddDrone(newDrone);
                                break;
                            case EntityOption.Customer:
                                Customer newCustomer = InputCustomer();
                                DalObject.DalObject.AddCustomer(newCustomer);
                                break;
                            case EntityOption.Parcel:
                                Parcel newParcel = InputParcel();
                                DalObject.DalObject.AddParcel(newParcel);
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
                                Console.WriteLine("Enter id of parcel to connect:\n");
                                PrintParcelsNoDrones();
                                int.TryParse(Console.ReadLine(), out idParcel);
                                Parcel parcel = DalObject.DalObject.GetParcel(idParcel);
                                Console.WriteLine("Enter id of drone:\n");
                                PrintDronesAvailable();
                                int.TryParse(Console.ReadLine(), out idDrone);
                                Drone drone = DalObject.DalObject.GetDrone(idDrone);
                                DalObject.DalObject.ConnectParcelToDrone(parcel, drone);
                                break;
                            case OptionUpdate.CollectionParcelByDrone:
                                Console.WriteLine("Enter id of parcel to PickedUp:\n");
                                PrintParcelsWithNoAssign();
                                int.TryParse(Console.ReadLine(), out idParcel);
                                Parcel helpParcel = DalObject.DalObject.GetParcel(idParcel);
                                DalObject.DalObject.CollectionParcelByDrone(helpParcel);
                                break;
                            case OptionUpdate.SupplyParcelToCustomer:
                                Console.WriteLine("Enter id of parcel to delivered:\n");
                                PrintParcelsPickedUp();
                                int.TryParse(Console.ReadLine(), out idParcel);
                                Parcel p = DalObject.DalObject.GetParcel(idParcel);
                                DalObject.DalObject.SupplyParcelToCustomer(p);
                                break;
                            case OptionUpdate.SendDroneToDroneCharge:
                                Console.WriteLine("Enter the id station:\n");
                                PrintStationsCharge();
                                int.TryParse(Console.ReadLine(), out idStation);
                                Station helpStation = DalObject.DalObject.GetStation(idStation);
                                Console.WriteLine("Enter id of drone:\n");
                                PrintDrones();
                                int.TryParse(Console.ReadLine(), out idDrone);
                                Drone helpDrone = DalObject.DalObject.GetDrone(idDrone);
                                DalObject.DalObject.SendDroneToDroneCharge(helpDrone, helpStation);
                                break;
                            case OptionUpdate.ReleaseDroneFromDroneCharge:
                                Console.WriteLine("Enter id of Station with place to charge:\n");
                                PrintStationsCharge();
                                int.TryParse(Console.ReadLine(), out idStation);
                                Station chargeStation = DalObject.DalObject.GetStation(idStation);
                                Console.WriteLine("Enter id of Drone to relese:\n");
                                PrintDronesCharge();
                                int.TryParse(Console.ReadLine(), out idDrone);
                                Drone releaseDrone = DalObject.DalObject.GetDrone(idDrone);
                                DalObject.DalObject.ReleaseDroneFromDroneCharge(chargeStation, releaseDrone);
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
                                Console.WriteLine(DalObject.DalObject.GetStation(myId).ToString());
                                break;
                            case EntityOption.Drone:
                                Console.WriteLine(DalObject.DalObject.GetDrone(myId).ToString());
                                break;
                            case EntityOption.Customer:
                                Console.WriteLine(DalObject.DalObject.GetCustomer(myId).ToString());
                                break;
                            case EntityOption.Parcel:
                                Console.WriteLine(DalObject.DalObject.GetParcel(myId).ToString());
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
                                PrintStations();
                                break;
                            case OptionListView.ListDrones:
                                PrintDrones();
                                break;
                            case OptionListView.ListCustomers:
                                PrintCustomers();
                                break;
                            case OptionListView.ListParcels:
                                PrintParcels();
                                break;
                            case OptionListView.ListParcelsNoDrones:
                                PrintParcelsNoDrones();
                                break;
                            case OptionListView.ListStationsCharge:
                                PrintStationsCharge();
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
            NewDrone.id = num;

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
            DateTime d;
            Parcel NewParcel = new Parcel();
            do
            {
                Console.WriteLine("Enter Id Parcel: ");

            } while (!int.TryParse(Console.ReadLine(), out num));
            NewParcel.id = num;

            do
            {
                Console.WriteLine("Enter Sender Id Parcel: ");
            } while (!int.TryParse(Console.ReadLine(), out num));
            NewParcel.senderId = num;

            do
            {
                Console.WriteLine("Enter Target Id Parcel: ");
            } while (!int.TryParse(Console.ReadLine(), out num));
            NewParcel.targetId = num;

            do
            {
                Console.WriteLine("Enter Weight Parcel:\n" + "1: Light\n" + "2: Medium\n" + "3: Heavy\n");
                int.TryParse(Console.ReadLine(), out num);
            } while (num != 1 && num != 2 && num != 3);
            switch (num)
            {
                case 1:
                    NewParcel.weight = WeightCategories.Light;
                    break;
                case 2:
                    NewParcel.weight = WeightCategories.Medium;
                    break;
                case 3:
                    NewParcel.weight = WeightCategories.Heavy;
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
                    NewParcel.priority = Priorities.Normal;
                    break;
                case 2:
                    NewParcel.priority = Priorities.Fast;
                    break;
                case 3:
                    NewParcel.priority = Priorities.Emergency;
                    break;
                default:
                    break;
            }

            IEnumerable<Drone> newDrones = DalObject.DalObject.GetDrones();

            if (newDrones.Count() != 0)
            {
                Console.WriteLine("Chooce a Drone forme the list:");
                PrintDronesAvailable();//print all the drones in the data
                do
                {
                    Console.WriteLine("Enter Drone Id frome the list to your Parcel: ");
                } while (!int.TryParse(Console.ReadLine(), out num) || !checkNotExistParcel(NewParcel, newDrones));
                NewParcel.droneId = num;
            }
            else
            {
                NewParcel.droneId = -1;
            }

            do
            {
                Console.WriteLine("Enter Requested Time Parcel: ");
            } while (!DateTime.TryParse(Console.ReadLine(), out d));
            NewParcel.requested = d;

            return NewParcel;
        }

        /// <summary>
        /// print all the list of stations
        /// </summary>
        /// <returns></no returns, just print>
        public static void PrintStations()//print the list
        {
            foreach (Station elementStation in DalObject.DalObject.GetStations())
                Console.WriteLine(elementStation.ToString());
        }

        /// <summary>
        /// print all the list of drones
        /// </summary>
        /// <returns></no returns, just print>
        public static void PrintDrones()//print the list
        {
            foreach (Drone elementDrone in DalObject.DalObject.GetDrones()) 
                Console.WriteLine(elementDrone.ToString());
        }

        /// <summary>
        /// print all the list of Drones Available
        /// </summary>
        /// <returns></no returns, just print>
        public static void PrintDronesAvailable()//print the list
        {
            IEnumerable<Drone> newDrones = DalObject.DalObject.GetDrones();
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
        public static void PrintDronesCharge()//print the list
        {
            foreach (DroneCharge elementDroneCharge in DalObject.DalObject.GetDronesCharge())
                Console.WriteLine(elementDroneCharge.ToString());
        }

        /// <summary>
        /// print all the list of customers
        /// </summary>
        /// <returns></no returns, just print>
        public static void PrintCustomers()//print the list
        {
            foreach (Customer elementCustomer in DalObject.DalObject.GetCustomers())
                Console.WriteLine(elementCustomer.ToString());
        }

        /// <summary>
        /// print all the list of parcels
        /// </summary>
        /// <returns></no returns, just print>
        public static void PrintParcels()//print the list
        {
            foreach (Parcel elementParcel in DalObject.DalObject.GetParcels())
                Console.WriteLine(elementParcel.ToString());
        }

        /// <summary>
        /// print all the list of Parcels With No Assign 
        /// </summary>
        /// <returns></no returns, just print>
        public static void PrintParcelsWithNoAssign()//print the list
        {
            foreach (Parcel elementParcel in DalObject.DalObject.GetParcels())
                if (elementParcel.droneId != -1)//the parcel wasnt connected
                    Console.WriteLine(elementParcel.ToString());
        }

        /// <summary>
        /// print all the list of Parcels Picked Up
        /// </summary>
        /// <returns></no returns, just print>
        public static void PrintParcelsPickedUp()//print the list
        {
            foreach (Parcel elementParcel in DalObject.DalObject.GetParcels())
                if (elementParcel.pickedUp != DateTime.MinValue)//the parcel was pickup
                    Console.WriteLine(elementParcel.ToString());
        }

        /// <summary>
        /// print all the list of Parcels No Drones
        /// </summary>
        /// <returns></no returns, just print>
        public static void PrintParcelsNoDrones()//print the list
        {
            foreach (Parcel elementParcel in DalObject.DalObject.GetParcels())
                if (elementParcel.droneId == -1)//the id drone is not exist
                    Console.WriteLine(elementParcel.ToString());
        }

        /// <summary>
        /// print all the list of Stations Charge
        /// </summary>
        /// <returns></no returns, just print>
        public static void PrintStationsCharge()//print the list
        {
            foreach (Station elementStation in DalObject.DalObject.GetStations())
                if (elementStation.chargeSlots > 0)
                    Console.WriteLine(elementStation.ToString());
        }
    }
}


