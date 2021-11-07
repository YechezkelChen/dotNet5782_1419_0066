using System;
using IBL;
using IBL.BO;

namespace ConsoleUI_BL
{
    class Program
    {
        public enum Option
        {
            Add = 1,
            Update,
            View,
            ListView,
            Exit
        };

        public enum EntityOption
        {
            Station = 1,
            Drone,
            Customer,
            Parcel,
            Exit
        };

        public enum OptionListView
        {
            ListStations = 1,
            ListDrones,
            ListCustomers,
            ListParcels,
            ListParcelsNoDrones,
            ListStationsCharge,
            Exit
        };

        public enum OptionUpdate
        {
            NameDrone = 1,
            DataStation,
            DataCustomer,
            SendDroneToDroneCharge,
            ReleaseDroneFromDroneCharge,
            ConnectParcelToDrone,
            CollectionParcelByDrone,
            SupplyParcelByDrone,
            Exit
        };

        static void Main(string[] args)
        {
            IBL.IBL bl = new BL();
            int c, myId;
            Option op;
            EntityOption ep;
            OptionListView olv;
            OptionUpdate ou;
            do
            {
                do
                {
                    Console.WriteLine("\nHELLO\n" + "Choose one of the following:\n" + "1: Add\n" + "2: Update\n" + "3: View\n" + "4: List View\n" + "5: Exit\n");
                } while (!int.TryParse(Console.ReadLine(), out c));
                op = (Option) c;
                try
                {
                    switch (op)
                    {
                        case Option.Add:
                            do
                            {
                                Console.WriteLine("Choose one of the entity:\n" + "1: Station\n" + "2: Drone\n" +
                                                  "3: Customer\n" + "4: Parcel\n" + "5: Exit\n");
                            } while (!int.TryParse(Console.ReadLine(), out c));
                            ep = (EntityOption)c;
                            switch (ep)
                            {
                                case EntityOption.Station:
                                    Station newStation = InputStation();
                                    bl.AddStation(newStation);
                                    break;
                                case EntityOption.Drone:
                                    Drone newDrone = InputDrone();
                                    bl.AddDrone(newDrone);
                                    break;
                                case EntityOption.Customer:
                                    Customer newCustomer = InputCustomer();
                                    bl.AddCustomer(newCustomer);
                                    break;
                                case EntityOption.Parcel:
                                    Parcel newParcel = InputParcel();
                                    bl.AddParcel(newParcel);
                                    break;
                                case EntityOption.Exit:
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case Option.Update:
                            do
                            {
                                Console.WriteLine("Choose one of to update:\n" + "1: NameDrone:\n" +
                                                  "2: DataStation\n" + "3: DataCustomer\n" +
                                                  "4: Send Drone To Drone Charge\n" +
                                                  "5: Release Drone From Drone Charge\n" + "6: ConnectParcelToDrone\n" +
                                                  "7: CollectionParcelByDrone\n" + "8: SupplyParcelByDrone\n" +
                                                  "9: Exit\n");
                            } while (!int.TryParse(Console.ReadLine(), out c));
                            ou = (OptionUpdate)c;
                            switch (ou)
                            {
                                case OptionUpdate.NameDrone:
                                    break;
                                case OptionUpdate.DataStation:
                                    break;
                                case OptionUpdate.DataCustomer:
                                    break;
                                case OptionUpdate.SendDroneToDroneCharge:
                                    break;
                                case OptionUpdate.ReleaseDroneFromDroneCharge:
                                    break;
                                case OptionUpdate.ConnectParcelToDrone:
                                    break;
                                case OptionUpdate.CollectionParcelByDrone:
                                    break;
                                case OptionUpdate.SupplyParcelByDrone:
                                    break;
                                case OptionUpdate.Exit:
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case Option.View:
                            do
                            {
                                Console.WriteLine("Choose one of the entity:\n" + "1: Station\n" + "2: Drone\n" +
                                                  "3: Customer\n" + "4: Parcel\n" + "5: Exit\n");
                            } while (!int.TryParse(Console.ReadLine(), out c));
                            ep = (EntityOption)c;
                            do
                            {
                                Console.WriteLine("Enter Id of the entity:\n");
                            } while (!int.TryParse(Console.ReadLine(), out myId));
                            switch (ep)
                            {
                                case EntityOption.Station:
                                    Console.WriteLine(bl.GetStation(myId).ToString());
                                    break;
                                case EntityOption.Drone:
                                    Console.WriteLine(bl.GetDrone(myId).ToString());
                                    break;
                                case EntityOption.Customer:
                                    Console.WriteLine(bl.GetCustomer(myId).ToString());
                                    break;
                                case EntityOption.Parcel:
                                    Console.WriteLine(bl.GetParcel(myId).ToString());
                                    break;
                                case EntityOption.Exit:
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case Option.ListView:
                            do
                            {
                                Console.WriteLine("Choose one of the entity:\n" + "1: List Stations\n" +
                                                  "2: List Drones\n" + "3: List Customers\n" + "4: List Parcels\n" +
                                                  "5: List Parcels No Drones\n" + "6: List Stations Charge\n" +
                                                  "7:Exit\n");
                            } while (!int.TryParse(Console.ReadLine(), out c));
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
                catch (Exception ex) //generally exception for all the options
                {
                    Console.WriteLine(ex);
                }
            } while (op != Option.Exit);
        }

        public static Station InputStation()
        {
            int num1;
            double num2,num3;
            Location stationLocation=new Location();
            Station NewStation = new Station();
            do
            {
                Console.WriteLine("Enter Id Station: ");
            } while (!int.TryParse(Console.ReadLine(), out num1));
            NewStation.Id = num1;

            do
            {
                Console.WriteLine("Enter Name Station: ");

            } while (!int.TryParse(Console.ReadLine(), out num1));
            NewStation.Name = num1;

            do
            {
                Console.WriteLine("Enter longitude Station: ");

            } while (!double.TryParse(Console.ReadLine(), out num2));

            stationLocation.Longitude = num2;

            do
            {
                Console.WriteLine("Enter Latitude Station: ");

            } while (!double.TryParse(Console.ReadLine(), out num2));
            stationLocation.Latitude = num2;

            NewStation.Location = stationLocation;

            do
            {
                Console.WriteLine("Enter ChargeSlots Station: ");

            } while (!int.TryParse(Console.ReadLine(), out num1));
            NewStation.ChargeSlots = num1;
            
            return NewStation;
        }
    }
}
