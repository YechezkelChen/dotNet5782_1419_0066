using System;
using DalObject;
using IDAL.DO;

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
            int c;
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
                                DalObject.DalObject.AddStation();
                                break;
                            case EntityOption.Drone:
                                Drone newDrone = InputDrone();
                                DalObject.DalObject.AddDrone();
                                break;
                            case EntityOption.Customer:
                                Customer newCustomer = InputCustomer();
                                DalObject.DalObject.AddCustomer();
                                break;
                            case EntityOption.Parcel:
                                Parcel newParcel = InputParcel();
                                DalObject.DalObject.AddParcel();
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
                                DalObject.DalObject.ConnectParcelToDrone();
                                break;
                            case OptionUpdate.CollectionParcelByDrone:
                                DalObject.DalObject.CollectionParcelByDrone();
                                break;
                            case OptionUpdate.SupplyParcelToCustomer:
                                DalObject.DalObject.SupplyParcelToCustomer();
                                break;
                            case OptionUpdate.SendDroneToDroneCharge:
                                int IdStation;
                                Console.WriteLine("Enter the id station:\n");
                                DalObject.DalObject.PrintStationsCharge();
                                int.TryParse(Console.ReadLine(), out IdStation);
                                DalObject.DalObject.SendDroneToDroneCharge(IdStation);
                                break;
                            case OptionUpdate.ReleaseDroneFromDroneCharge:
                                DalObject.DalObject.ReleaseDroneFromDroneCharge();
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
                        int MyId;
                        Console.WriteLine("Enter Id of the entity:\n");
                        int.TryParse(Console.ReadLine(), out MyId);
                        switch (ep)
                        {
                            case EntityOption.Station:
                                Console.WriteLine(DalObject.DalObject.GetStation(MyId).ToString());
                                break;
                            case EntityOption.Drone:
                                Console.WriteLine(DalObject.DalObject.GetDrone(MyId).ToString());
                                break;
                            case EntityOption.Customer:
                                Console.WriteLine(DalObject.DalObject.GetCustomer(MyId).ToString());
                                break;
                            case EntityOption.Parcel:
                                Console.WriteLine(DalObject.DalObject.GetParcel(MyId).ToString());
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
                                DalObject.DalObject.PrintStations();
                                break;
                            case OptionListView.ListDrones:
                                DalObject.DalObject.PrintDrones();
                                break;
                            case OptionListView.ListCustomers:
                                DalObject.DalObject.PrintCustomers();
                                break;
                            case OptionListView.ListParcels:
                                DalObject.DalObject.PrintParcels();
                                break;
                            case OptionListView.ListParcelsNoDrones:
                                DalObject.DalObject.PrintParcelsNoDrones();
                                break;
                            case OptionListView.ListStationsCharge:
                                DalObject.DalObject.PrintStationsCharge();
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

        public static Station InputStation()
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

            return NewStation;
        }

        public static Drone InputDrone()
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
            } while (num != 1 && num != 2 && num != 3);
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
            } while (num != 1 && num != 2 && num != 3);
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

            return NewDrone;
        }

        public static Customer InputCustomer()
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

            return NewCustomer;
        }

        public static Parcel InputParcel()
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
            if (DataSource.Config.VacantIndexD != 0)
            {
                Console.WriteLine("Chooce a Drone forme the list:");
                PrintDronesAvailable();//print all the drones in the data
                Console.WriteLine("Enter Drone Id frome the list to your Parcel: ");
                int.TryParse(Console.ReadLine(), out num);
                bool flag = false;
                for (int i = 0; i < DataSource.Config.VacantIndexD; i++)
                {
                    if (num == DataSource.Drones[i].Id)
                        flag = true;
                }
                if (!flag)//the drone the user whant is not in the list
                    NewParcel.DroneId = -1;//parcel that not have drone
                else
                    NewParcel.DroneId = num;
            }
            else
            {
                NewParcel.DroneId = -1;
            }
            Console.WriteLine("Enter Requested Time Parcel: ");
            DateTime.TryParse(Console.ReadLine(), out d);
            NewParcel.Requested = d;

            return NewParcel;
        }
    }
}


