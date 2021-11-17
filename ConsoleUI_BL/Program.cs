using System;
using System.Collections.Generic;
using System.Threading.Channels;
using IBL;
using IBL.BO;
using IDAL.DO;
using Customer = IBL.BO.Customer;
using Drone = IBL.BO.Drone;
using Parcel = IBL.BO.Parcel;
using Station = IBL.BO.Station;

namespace ConsoleUI_BL
{
    internal class Program
    {
        private enum Option
        {
            Add = 1,
            Update,
            View,
            ListView,
            Exit
        };

        private enum EntityOption
        {
            Station = 1,
            Drone,
            Customer,
            Parcel,
            Exit
        };

        private enum OptionListView
        {
            ListStations = 1,
            ListDrones,
            ListCustomers,
            ListParcels,
            ListParcelsNoDrones,
            ListStationsCharge,
            Exit
        };

        private enum OptionUpdate
        {
            ModelDrone = 1,
            DataStation,
            DataCustomer,
            SendDroneToDroneCharge,
            ReleaseDroneFromDroneCharge,
            ConnectParcelToDrone,
            CollectionParcelByDrone,
            SupplyParcelByDrone,
            Exit
        };

        private static void Main(string[] args)
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
                    Console.WriteLine("HELLO\n" + "Choose one of the following:\n" + "1: Add\n" + "2: Update\n" +
                                      "3: View\n" + "4: List View\n" + "5: Exit\n");
                } while (!int.TryParse(Console.ReadLine(), out c));

                op = (Option) c;
                switch (op)
                {
                    case Option.Add:
                        do
                        {
                            Console.WriteLine("Choose one of the entity:\n" + "1: Station\n" + "2: Drone\n" +
                                              "3: Customer\n" + "4: Parcel\n" + "5: Exit\n");
                        } while (!int.TryParse(Console.ReadLine(), out c));

                        ep = (EntityOption) c;
                        switch (ep)
                        {
                            case EntityOption.Station:
                                AddStation(bl);
                                break;
                            case EntityOption.Drone:
                                AddDrone(bl);
                                break;
                            case EntityOption.Customer:
                                AddCustomer(bl);
                                break;
                            case EntityOption.Parcel:
                                AddParcel(bl);
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
                            Console.WriteLine("Choose one of to update:\n" + "1: ModelDrone:\n" +
                                              "2: DataStation\n" + "3: DataCustomer\n" +
                                              "4: Send Drone To Drone Charge\n" +
                                              "5: Release Drone From Drone Charge\n" + "6: ConnectParcelToDrone\n" +
                                              "7: CollectionParcelByDrone\n" + "8: SupplyParcelByDrone\n" +
                                              "9: Exit\n");
                        } while (!int.TryParse(Console.ReadLine(), out c));

                        ou = (OptionUpdate) c;
                        switch (ou)
                        {
                            case OptionUpdate.ModelDrone:
                                UpdateDroneModel(bl);
                                break;
                            case OptionUpdate.DataStation:
                                UpdateDataStation(bl);
                                break;
                            case OptionUpdate.DataCustomer:
                                UpdateDataCustomer(bl);
                                break;
                            case OptionUpdate.SendDroneToDroneCharge:
                                SendDroneToCharge(bl);
                                break;
                            case OptionUpdate.ReleaseDroneFromDroneCharge:
                                ReleaseDroneFromCharge(bl);
                                break;
                            case OptionUpdate.ConnectParcelToDrone:
                                ConnectParcelToDrone(bl);
                                break;
                            case OptionUpdate.CollectionParcelByDrone:
                                CollectionParcelByDrone(bl);
                                break;
                            case OptionUpdate.SupplyParcelByDrone:
                                SupplyParcelByDrone(bl);
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

                        ep = (EntityOption) c;
                        myId = 0;
                        if(c != 5)
                            do
                            {
                                Console.WriteLine("Enter Id of the entity:\n");
                            } while (!int.TryParse(Console.ReadLine(), out myId));

                        switch (ep)
                        {
                            case EntityOption.Station:
                                try
                                {
                                    Console.WriteLine(bl.GetStation(myId));
                                }
                                catch (StationException e)
                                {
                                    Console.WriteLine(e);
                                }
                                break;
                            case EntityOption.Drone:
                                try
                                {
                                    Console.WriteLine(bl.GetDrone(myId));
                                }
                                catch (DroneException e)
                                {
                                    Console.WriteLine(e);
                                }
                                break;
                            case EntityOption.Customer:
                                try
                                {
                                    Console.WriteLine(bl.GetCustomer(myId));
                                }
                                catch (CustomerException e)
                                {
                                    Console.WriteLine(e);

                                }
                                break;
                            case EntityOption.Parcel:
                                try
                                {
                                    Console.WriteLine(bl.GetParcel(myId));
                                }
                                catch (ParcelException e)
                                {
                                    Console.WriteLine(e);
                                }
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

                        olv = (OptionListView) c;
                        switch (olv)
                        {
                            case OptionListView.ListStations:
                                try
                                {
                                    foreach (var elementStation in bl.GetStations())
                                        Console.WriteLine(elementStation.ToString());
                                }
                                catch (StationException e)
                                {
                                    Console.WriteLine(e);
                                }
                                break;
                            case OptionListView.ListDrones:
                                try
                                {
                                    foreach (var elementDrone in bl.GetDrones())
                                        Console.WriteLine(elementDrone.ToString());
                                }
                                catch (DroneException e)
                                {
                                    Console.WriteLine(e); 
                                }
                                break;
                            case OptionListView.ListCustomers:
                                try
                                {
                                    foreach (var elementCustomer in bl.GetCustomers())
                                        Console.WriteLine(elementCustomer.ToString());
                                }
                                catch (CustomerException e)
                                {
                                    Console.WriteLine(e);
                                }
                                break;
                            case OptionListView.ListParcels:
                                try
                                {
                                    foreach (var elementParcel in bl.GetParcels())
                                        Console.WriteLine(elementParcel.ToString());
                                }
                                catch (ParcelException e)
                                {
                                    Console.WriteLine(e);
                                }
                                break;
                            case OptionListView.ListParcelsNoDrones:
                                try
                                {
                                    foreach (var elementParcelsNoDrone in bl.GetParcelsNoDrones())
                                        Console.WriteLine(elementParcelsNoDrone.ToString());
                                }
                                catch (ParcelException e)
                                {
                                    Console.WriteLine(e);
                                }
                                break;
                            case OptionListView.ListStationsCharge:
                                try
                                {
                                    foreach (var elementStationCharge in bl.GetStationsCharge())
                                        Console.WriteLine(elementStationCharge.ToString());
                                }
                                catch (StationException e)
                                {
                                    Console.WriteLine(e);
                                }
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
            while (op != Option.Exit);
        }

        
        /// <summary>
        /// read from the user station to insert to list
        /// </summary>
        /// <returns></no returns, just read from user>
        private static void AddStation(IBL.IBL bl)
        {
            int num1;
            double num2;
            Location stationLocation = new Location();
            Station newStation = new Station();
            while (true)
            {
                do
                {
                    Console.WriteLine("Enter Id Station: ");
                } while (!int.TryParse(Console.ReadLine(), out num1));

                newStation.Id = num1;

                do
                {
                    Console.WriteLine("Enter Name Station: ");

                } while (!int.TryParse(Console.ReadLine(), out num1));

                newStation.Name = num1;

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
                newStation.Location = stationLocation;

                do
                {
                    Console.WriteLine("Enter Available Charge Slots  Station: ");

                } while (!int.TryParse(Console.ReadLine(), out num1));

                newStation.ChargeSlots = num1;

                newStation.InCharges = new List<DroneCharge>();

                try
                {
                    bl.AddStation(newStation);
                    Console.WriteLine("Success! :)\n");
                    break;
                }
                catch (StationException e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Do you want to add station? please enter yes or no.");
                    string choice = Console.ReadLine();
                    if (choice != "yes")
                        break;
                }
            }
        }

        /// <summary>
        /// read fron the user drone to insert to list
        /// </summary>
        /// <returns></no returns, just read from user>
        private static void AddDrone(IBL.IBL bl)
        {
            int num;
            Drone newDrone = new Drone();
            while (true)
            {


                do
                {
                    Console.WriteLine("Enter Id Drone: ");
                } while (!int.TryParse(Console.ReadLine(), out num));

                newDrone.Id = num;

                Console.WriteLine("Enter Model Drone: ");
                newDrone.Model = Console.ReadLine();

                do
                {
                    Console.WriteLine("Enter Weight Drone:\n" + "1: Light\n" + "2: Medium\n" + "3: Heavy\n");
                    int.TryParse(Console.ReadLine(), out num);
                } while (num != 1 && num != 2 && num != 3);

                switch (num)
                {
                    case 1:
                        newDrone.Weight = IBL.BO.WeightCategories.Light;
                        break;
                    case 2:
                        newDrone.Weight = IBL.BO.WeightCategories.Medium;
                        break;
                    case 3:
                        newDrone.Weight = IBL.BO.WeightCategories.Heavy;
                        break;
                    default:
                        break;
                }

                do
                {
                    Console.WriteLine("Enter id of station to put the drone in: ");
                } while (!int.TryParse(Console.ReadLine(), out num));

                try
                {
                    bl.AddDrone(newDrone, num);
                    Console.WriteLine("Success! :)\n");
                }
                catch (DroneException e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Do you want to add drone? please enter yes or no.");
                    string choice = Console.ReadLine();
                    if (choice != "yes")
                        break;
                }
                catch (StationException e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Do you want to add drone? please enter yes or no.");
                    string choice = Console.ReadLine();
                    if (choice != "yes")
                        break;
                }
            }
        }

        /// <summary>
        /// read fron the user customer to insert to list
        /// </summary>
        /// <returns></no returns, just read from user>
        private static void AddCustomer(IBL.IBL bl)
        {
            int num1;
            double num2;
            Location customerLocation = new Location();
            Customer newCustomer = new Customer();

            while (true)
            {
                do
                {
                    Console.WriteLine("Enter Id with 8 digits of Customer: ");
                } while (!int.TryParse(Console.ReadLine(), out num1));

                newCustomer.Id = num1;

                Console.WriteLine("Enter Name Customer: ");
                newCustomer.Name = Console.ReadLine();

                Console.WriteLine("Enter Phone Customer: ");
                newCustomer.Phone = Console.ReadLine();

                do
                {
                    Console.WriteLine("Enter Longitude Customer: ");
                } while (!double.TryParse(Console.ReadLine(), out num2));

                customerLocation.Longitude = num2;

                do
                {
                    Console.WriteLine("Enter Latitude Customer: ");
                } while (!double.TryParse(Console.ReadLine(), out num2));

                customerLocation.Latitude = num2;
                newCustomer.Location = customerLocation;

                try
                {
                    bl.AddCustomer(newCustomer);
                    Console.WriteLine("Success! :)\n");
                    break;
                }
                catch (CustomerException e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Do you want to add customer? please enter yes or no.");
                    string choice = Console.ReadLine();
                    if (choice != "yes")
                        break;
                }
            }
        }

        /// <summary>
        /// read from the user parcel to insert to list
        /// </summary>
        /// <returns></no returns, just read from user>
        private static void AddParcel(IBL.IBL bl)
        {
            int num;
            Parcel newParcel = new Parcel();

            while (true)
            {


                do
                {
                    Console.WriteLine("Enter Sender Id Parcel: ");
                } while (!int.TryParse(Console.ReadLine(), out num));

                newParcel.Sender = new CustomerInParcel();
                newParcel.Sender.Id = num;

                do
                {
                    Console.WriteLine("Enter Target Id Parcel: ");
                } while (!int.TryParse(Console.ReadLine(), out num));

                newParcel.Target = new CustomerInParcel();
                newParcel.Target.Id = num;

                do
                {
                    Console.WriteLine("Enter Weight Parcel:\n" + "1: Light\n" + "2: Medium\n" + "3: Heavy\n");
                    int.TryParse(Console.ReadLine(), out num);
                } while (num != 1 && num != 2 && num != 3);

                switch (num)
                {
                    case 1:
                        newParcel.Weight = IBL.BO.WeightCategories.Light;
                        break;
                    case 2:
                        newParcel.Weight = IBL.BO.WeightCategories.Medium;
                        break;
                    case 3:
                        newParcel.Weight = IBL.BO.WeightCategories.Heavy;
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
                        newParcel.Priority = IBL.BO.Priorities.Normal;
                        break;
                    case 2:
                        newParcel.Priority = IBL.BO.Priorities.Fast;
                        break;
                    case 3:
                        newParcel.Priority = IBL.BO.Priorities.Emergency;
                        break;
                    default:
                        break;
                }

                try
                {
                    bl.AddParcel(newParcel);
                    Console.WriteLine("Success! :)\n");
                    break;
                }
                catch (ParcelException e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Do you want to add parcel? please enter yes or no.");
                    string choice = Console.ReadLine();
                    if (choice != "yes")
                        break;
                }
            }
        }

        private static void UpdateDroneModel(IBL.IBL bl)
        {
            int id;
            string model;
            while (true)
            {


                do
                {
                    Console.WriteLine("Enter id drone: ");
                } while (!int.TryParse(Console.ReadLine(), out id));

                Console.WriteLine("Enter the new model drone: ");
                model = Console.ReadLine();

                try
                {
                    bl.UpdateDroneModel(id, model);
                    Console.WriteLine("Success! :)\n");
                    break;
                }
                catch (DroneException e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Do you want to update the model of the drone? please enter yes or no.");
                    string choice = Console.ReadLine();
                    if (choice != "yes")
                        break;
                }
            }
        }

        private static void UpdateDataStation(IBL.IBL bl)
        {
            int id, name, chargeSlots;

            while (true)
            {
                do
                {
                    Console.WriteLine("Enter Id Station: ");
                } while (!int.TryParse(Console.ReadLine(), out id));

                Console.WriteLine("Note! Enter at least one of the following data:\n");
                do
                {
                    Console.WriteLine(
                        "If you want to update the name of the station, Enter name:\n if you don't want, Enter -1: \n");
                } while (!int.TryParse(Console.ReadLine(), out name));

                do
                {
                    Console.WriteLine(
                        "If you want to update the number of charge Slots in the station, Enter the amount:\n if you don't want, Enter -1: \n");
                } while (!int.TryParse(Console.ReadLine(), out chargeSlots));

                try
                {
                    bl.UpdateDataStation(id, name, chargeSlots);
                    Console.WriteLine("Success! :)\n");
                    break;
                }
                catch (StationException e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Do you want to update the data of the station? please enter yes or no.");
                    string choice = Console.ReadLine();
                    if (choice != "yes")
                        break;
                }
            }
        }

        private static void UpdateDataCustomer(IBL.IBL bl)
        {
            int id;
            string name, phone;

            while (true)
            {
                do
                {
                    Console.WriteLine("Enter Id Customer: ");
                } while (!int.TryParse(Console.ReadLine(), out id));

                Console.WriteLine("Note! Enter at least one of the following data:\n");

                Console.WriteLine(
                    "If you want to update the name of the customer, Enter name:\n if you don't want, press Enter: \n");
                name = Console.ReadLine();

                Console.WriteLine(
                    "If you want to update the phone of the customer, Enter phone:\n if you don't want, press Enter: \n");
                phone = Console.ReadLine();

                try
                {
                    bl.UpdateDataCustomer(id, name, phone);
                    Console.WriteLine("Success! :)\n");
                    break;
                }
                catch (CustomerException e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Do you want to update the data of the customer? please enter yes or no.");
                    string choice = Console.ReadLine();
                    if (choice != "yes")
                        break;
                }
            }
        }

        private static void SendDroneToCharge(IBL.IBL bl)
        {
            int id;

            while (true)
            {


                do
                {
                    Console.WriteLine("Enter Id Drone: ");
                } while (!int.TryParse(Console.ReadLine(), out id));

                try
                {
                    bl.SendDroneToDroneCharge(id);
                    Console.WriteLine("Success! :)\n");
                    break;
                }
                catch (DroneException e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Do you want to send the drone to charge? please enter yes or no.");
                    string choice = Console.ReadLine();
                    if (choice != "yes")
                        break;
                }
            }
        }
        
        private static void ReleaseDroneFromCharge(IBL.IBL bl)
        {
            int id, chargeTime;

            while (true)
            {
                do
                {
                    Console.WriteLine("Enter Id Drone: ");
                } while (!int.TryParse(Console.ReadLine(), out id));

                do
                {
                    Console.WriteLine("Enter the time the drone was in charge:\n ");
                } while (!int.TryParse(Console.ReadLine(), out chargeTime));

                try
                {
                    bl.ReleaseDroneFromDroneCharge(id, chargeTime);
                    Console.WriteLine("Success! :)\n");
                    break;
                }
                catch (DroneException e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Do you want to release the drone from charge? please enter yes or no.");
                    string choice = Console.ReadLine();
                    if (choice != "yes")
                        break;
                }
            }
        }

        private static void ConnectParcelToDrone(IBL.IBL bl)
        {
            int droneId;

            while (true)
            {
                do
                {
                    Console.WriteLine("Enter Id Drone: ");
                } while (!int.TryParse(Console.ReadLine(), out droneId));

                try
                {
                    bl.ConnectParcelToDrone(droneId);
                    Console.WriteLine("Success! :)\n");
                    break;
                }
                catch (DroneException e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Do you want to connect parcel to drone? please enter yes or no.");
                    string choice = Console.ReadLine();
                    if (choice != "yes")
                        break;
                }
            }
        }

        private static void CollectionParcelByDrone(IBL.IBL bl)
        {
            int droneId;

            while (true)
            {
                do
                {
                    Console.WriteLine("Enter Id Drone: ");
                } while (!int.TryParse(Console.ReadLine(), out droneId));

                try
                {
                    bl.CollectionParcelByDrone(droneId);
                    Console.WriteLine("Success! :)\n");
                    break;
                }
                catch (DroneException e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Do you want to collect the parcel by drone? please enter yes or no.");
                    string choice = Console.ReadLine();
                    if (choice != "yes")
                        break;
                }
            }
        }

        private static void SupplyParcelByDrone(IBL.IBL bl)
        {
            int droneId;

            while (true)
            {
                do
                {
                    Console.WriteLine("Enter Id Drone: ");
                } while (!int.TryParse(Console.ReadLine(), out droneId));

                try
                {
                    bl.SupplyParcelByDrone(droneId);
                    Console.WriteLine("Success! :)\n");
                    break;
                }
                catch (DroneException e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Do you want to supply parcel by drone? please enter yes or no.");
                    string choice = Console.ReadLine();
                    if (choice != "yes")
                        break;
                }
            }
        }
    }
}
